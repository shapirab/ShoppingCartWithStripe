using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.DbContexts;
using ShoppingCart.data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AppDbContext db;

        public ProductTypeService(AppDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task AddProductTypeAsync(ProductTypeEntity productType)
        {
            await db.ProductTypes.AddAsync(productType);
        }

        public async Task DeleteProductTypeAsync(int id)
        {
            ProductTypeEntity? productTypeEntity = await db.ProductTypes.FindAsync(id);
            if (productTypeEntity != null)
            {
                db.ProductTypes.Remove(productTypeEntity);
            }
        }

        public async Task<IEnumerable<ProductTypeEntity>> GetAllProductTypesAsync()
        {
            return await db.ProductTypes.OrderBy(prod => prod.Name).ToListAsync();
        }

        public async Task<(IEnumerable<ProductTypeEntity>, PaginationMetaData)> GetAllProductTypesAsync
            (string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<ProductTypeEntity> collection = db.ProductTypes as IQueryable<ProductTypeEntity>;

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

        public async Task<ProductTypeEntity?> GetProductTypeByIdAsync(int id)
        {
            return await db.ProductTypes.FindAsync(id);
        }

        public async Task<bool> IsProductTypeExists(int id)
        {
            ProductTypeEntity? productType = await GetProductTypeByIdAsync(id);
            return productType != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
