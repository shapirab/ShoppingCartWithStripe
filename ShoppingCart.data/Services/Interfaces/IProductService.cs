using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<(IEnumerable<ProductEntity>, PaginationMetaData)> GetAllProductsAsync
            (string? searchQuery, int? brandId, int? typeId, string? sort, int pageNumber, int pageSize);
        Task<ProductEntity?> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductEntity product);
        Task DeleteProductAsync(int id);
        Task<bool> IsProductExists(int id);
        Task<bool> SaveChangesAsync();
    }
}
