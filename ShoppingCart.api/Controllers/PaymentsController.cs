using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Implementations;
using ShoppingCart.data.Services.Interfaces;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService, 
        IDeliveryMethodService deliveryMethodService) : ControllerBase
    {
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

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodEntity>>> GetDeliveryMethods()
        {
            return Ok(await deliveryMethodService.GetAllDeliveryMethodsAsync());
        }
    }
}
