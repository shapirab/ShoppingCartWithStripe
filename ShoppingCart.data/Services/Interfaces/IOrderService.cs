using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<(IEnumerable<Order>, PaginationMetaData)> GetAllOrdersAsync
            (string? searchQuery, DateTime? orderDate, OrderStatus? orderStatus, int pageNumber, int pageSize);
        Task<IEnumerable<Order>> GetOrdersForUserAsync(string email);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderByPaymentIntentIdAsync(string id);
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<bool> IsOrderExists(int id);
        Task<bool> SaveChangesAsync();
    }
}
