using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IProductBrandService
    {
        Task<IEnumerable<ProductBrandEntity>> GetAllProductBrandsAsync();
        Task<(IEnumerable<ProductBrandEntity>, PaginationMetaData)> GetAllProductBrandsAsync
            (string? searchQuery, int pageNumber, int pageSize);
        Task<ProductBrandEntity?> GetProductBrandByIdAsync(int id);
        Task AddProductBrandAsync(ProductBrandEntity product);
        Task DeleteProductBrandAsync(int id);
        Task<bool> IsProductBrandExists(int id);
        Task<bool> SaveChangesAsync();
    }
}
