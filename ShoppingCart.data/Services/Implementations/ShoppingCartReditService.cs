using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class ShoppingCartReditService : ICartService
    {
        private readonly IDatabase db;

        public ShoppingCartReditService(IConnectionMultiplexer redis)
        {
            if(redis == null)
            {
                throw new ArgumentNullException(nameof(redis));
            }

            db = redis.GetDatabase();
        }

        public async Task<ShoppingCartModel?> AddOrUpdateShoppingCartAsync(ShoppingCartModel shoppingCart)
        {
            var created = await db.StringSetAsync(shoppingCart.Id, 
                JsonSerializer.Serialize(shoppingCart), TimeSpan.FromDays(30));

            if (!created)
            {
                return null;
            }
            return await GetShoppingCartAsync(shoppingCart.Id);
        }

        public async Task<bool> DeleteShoppingCartAsync(string key)
        {
            return await db.KeyDeleteAsync(key);
        }

        public async Task<ShoppingCartModel?> GetShoppingCartAsync(string key)
        {
            string? data = await db.StringGetAsync(key);

            return data == null ? null : JsonSerializer.Deserialize<ShoppingCartModel?>(data);
        }
    }
}
