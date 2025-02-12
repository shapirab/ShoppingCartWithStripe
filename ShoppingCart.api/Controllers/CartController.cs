using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Interfaces;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartModel>> GetShoppingCartById(string id)
        {
            ShoppingCartModel? shoppingCart = await cartService.GetShoppingCartAsync(id);
            //NOTE: we are not saving the cart to redis at this point! We are simply returning a new cart
            return Ok(shoppingCart?? new ShoppingCartModel { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartModel>> AddOrUpdateCart(ShoppingCartModel shoppingCart)
        {
            ShoppingCartModel? updatedCart = await cartService.AddOrUpdateShoppingCartAsync(shoppingCart);
            if(updatedCart == null)
            {
                return BadRequest("Shopping cart was not updated correctly");
            }
            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            bool results = await cartService.DeleteShoppingCartAsync(id);
            if (!results)
            {
                return BadRequest("Problem deleting cart");
            }
            return Ok();
        }
    }
}
