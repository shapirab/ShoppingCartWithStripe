using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeEntity>> GetAllProductTypesAsync();
        Task<(IEnumerable<ProductTypeEntity>, PaginationMetaData)> GetAllProductTypesAsync
            (string? searchQuery, int pageNumber, int pageSize);
        Task<ProductTypeEntity?> GetProductTypeByIdAsync(int id);
        Task AddProductTypeAsync(ProductTypeEntity productType);
        Task DeleteProductTypeAsync(int id);
        Task<bool> IsProductTypeExists(int id);
        Task<bool> SaveChangesAsync();
    }
}
