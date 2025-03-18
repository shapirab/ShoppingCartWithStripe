using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShoppingCart.api.SignalR;
using ShoppingCart.data.DataModels.Dtos.OrderDtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using ShoppingCart.data.Services.Implementations;
using ShoppingCart.data.Services.Interfaces;
using Stripe;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(
            IPaymentService paymentService, 
            IDeliveryMethodService deliveryMethodService, 
            IOrderService orderService,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper,
            ILogger<PaymentsController> logger, 
            IConfiguration config
        ) : ControllerBase
    {
        private readonly string whSecret = config["StripeSettings:whSecret"]!;

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCartModel>> CreateOrUpdatePaymentIntent(string cartId)
        {
            ShoppingCartModel? cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null)
            {
                return BadRequest("Problem with your cart");
            }
            return Ok(cart);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("refund/{id:int}")]
        public async Task<ActionResult<ReturnOrderDto>> RefundOrder(int id)
        {
            Order? order = await orderService.GetOrderByIdAsync(id);
            if(order == null)
            {
                return BadRequest("Order with this id was not found");
            }
            if(order.OrderStatus == OrderStatus.PENDING)
            {
                return BadRequest("Payment was not received for that order");
            }

            string? results = await paymentService.RefundPayment(order.PaymentIntentId);
            if(results == "succeeded")
            {
                order.OrderStatus = OrderStatus.REFUNDED;
                await orderService.SaveChangesAsync();
                return Ok(mapper.Map<ReturnOrderDto>(order));
            }
            return BadRequest("Problem in refunding order");
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodEntity>>> GetDeliveryMethods()
        {
            return Ok(await deliveryMethodService.GetAllDeliveryMethodsAsync());
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            string? json = await new StreamReader(Request.Body).ReadToEndAsync();
            try
            {
                Event stripeEvent = ConstructStripeEvent(json);
                if(stripeEvent.Data.Object is not PaymentIntent intent)
                {
                    return BadRequest("Invalid event data");
                }
                await HandlePaymentIntentSucceeded(intent);
                
                return Ok();
            }
            catch (StripeException ex) 
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error");
            }
        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], whSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct event");
                throw new StripeException("Invalid signature");
            }
        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if(intent.Status == "succeeded")
            {
                Order? order = await orderService.GetOrderByPaymentIntentIdAsync(intent.Id) ??
                    throw new Exception("Order not found");
                if((long)order.GetTotal() * 100 != intent.Amount)
                {
                    order.OrderStatus = OrderStatus.PAYMENT_MISMATCH;
                }
                else
                {
                    order.OrderStatus = OrderStatus.PAYMENT_RECIEVED;
                }
                await orderService.SaveChangesAsync();

                string? connectionId = NotificationHub.GetConnectionIdByEmail(order.CustomerEmail);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    ReturnOrderDto orderDto = mapper.Map<ReturnOrderDto>(order);
                    await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", orderDto);
                }
            }
        }
    }
}
