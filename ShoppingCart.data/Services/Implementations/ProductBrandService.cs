using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DbContexts;
using ShoppingCart.data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly AppDbContext db;

        public ProductBrandService(AppDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddProductBrandAsync(ProductBrandEntity productBrand)
        {
            await db.ProductBrands.AddAsync(productBrand);
        }

        public async Task DeleteProductBrandAsync(int id)
        {
            ProductBrandEntity? productBrandEntity = await db.ProductBrands.FindAsync(id);
            if (productBrandEntity != null)
            {
                db.ProductBrands.Remove(productBrandEntity);
            }
        }

        public async Task<IEnumerable<ProductBrandEntity>> GetAllProductBrandsAsync()
        {
            return await db.ProductBrands.OrderBy(prod => prod.Name).ToListAsync();
        }

        public async Task<(IEnumerable<ProductBrandEntity>, PaginationMetaData)> GetAllProductBrandsAsync
            (string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<ProductBrandEntity> collection = db.ProductBrands as IQueryable<ProductBrandEntity>;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(prod => prod.Name.Contains(searchQuery));
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(prod => prod.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collection, paginationMetadata);
        }

        public async Task<ProductBrandEntity?> GetProductBrandByIdAsync(int id)
        {
            return await db.ProductBrands.FindAsync(id);
        }

        public async Task<bool> IsProductBrandExists(int id)
        {
            ProductBrandEntity? productBrand = await GetProductBrandByIdAsync(id);
            return productBrand != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
