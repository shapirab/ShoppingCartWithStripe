using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.data.DataModels.Dtos.OrderDtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using ShoppingCart.data.Services.Interfaces;
using System.Security.Claims;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;
        private readonly IProductService productService;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, ICartService cartService,
            IProductService productService, IDeliveryMethodService deliveryMethodService, IMapper mapper)
        {
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.deliveryMethodService = deliveryMethodService ?? throw new ArgumentNullException(nameof(deliveryMethodService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("User email was not found");
            }
            ShoppingCartModel? cart = await cartService.GetShoppingCartAsync(orderDto.CartId);
            if (cart == null)
            {
                return BadRequest("Cart not found");
            }
            if (cart.PaymentIntentId == null)
            {
                return BadRequest("No payment intent for this order");
            }

            List<OrderItem> items = new List<OrderItem>();

            foreach (CartItem item in cart.Items)
            {
                ProductEntity? productItem = await productService.GetProductByIdAsync(item.ProductId);
                if (productItem == null)
                {
                    return BadRequest("Problem with order");
                }

                ProductItemOrdered itemOrdered = new ProductItemOrdered
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ImageUrl = item.ImageUrl,
                };
                OrderItem orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity,
                };
                items.Add(orderItem);
            }

            DeliveryMethodEntity? deliveryMethod =
                await deliveryMethodService.GetDeliveryMethodByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod == null)
            {
                return BadRequest("No delivery method selected");
            }

            Order order = new Order
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = items.Sum(prod => prod.Price * prod.Quantity),
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                CustomerEmail = email
            };

            await orderService.AddOrderAsync(order);
            bool saved = await orderService.SaveChangesAsync();
            if (!saved)
            {
                return BadRequest("Problem creating order");
            }
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnOrderDto>>> GetAllOrdersAsync()
        {
            IEnumerable<Order> orders = await orderService.GetAllOrdersAsync();
            return Ok(mapper.Map<IEnumerable<ReturnOrderDto>>(orders));
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ReturnOrderDto>>> GetOrdersForUser()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("User email was not found");
            }
            IEnumerable<Order> customerOrders = await orderService.GetOrdersForUserAsync(email);
            return Ok(mapper.Map<IEnumerable<ReturnOrderDto>>(customerOrders));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReturnOrderDto>> GetOrderById(int id)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("User email was not found");
            }
            Order? order = await orderService.GetOrderByIdAsync(id);
            if(order == null)
            {
                return NotFound("Order with provided id was not found");
            }
            return Ok(mapper.Map<ReturnOrderDto>(order));
        }
    }
}
