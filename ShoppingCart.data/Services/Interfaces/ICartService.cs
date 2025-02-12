using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.data.DataModels.Models;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface ICartService
    {
        Task<ShoppingCartModel?> GetShoppingCartAsync(string key);
        Task<ShoppingCartModel?> AddOrUpdateShoppingCartAsync(ShoppingCartModel shoppingCart);
        Task<bool> DeleteShoppingCartAsync(string key);

    }
}
