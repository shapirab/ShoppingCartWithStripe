using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IDeliveryMethodService
    {
        Task<IEnumerable<DeliveryMethodEntity>> GetAllDeliveryMethodsAsync();
        Task<(IEnumerable<DeliveryMethodEntity>, PaginationMetaData)> GetAllDeliveryMethodsAsync
            (string? searchQuery, string? sort, int pageNumber, int pageSize);
        Task<DeliveryMethodEntity?> GetDeliveryMethodByIdAsync(int id);
        Task AddDeliveryMethodAsync(DeliveryMethodEntity deliveryMethod);
        Task DeleteDeliveryMethodAsync(int id);
        Task<bool> IsDeliveryMethodExists(int id);
        Task<bool> SaveChangesAsync();
    }
}
