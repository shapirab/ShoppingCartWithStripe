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
    public class ProductService : IProductService
    {
        private readonly AppDbContext db;

        public ProductService(AppDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddProductAsync(ProductEntity product)
        {
            await db.Products.AddAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            ProductEntity? productEntity = await db.Products.FindAsync(id);
            if (productEntity != null)
            {
                db.Products.Remove(productEntity);
            }
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            return await db.Products.OrderBy(prod => prod.Name).ToListAsync();
        }

        public async Task<(IEnumerable<ProductEntity>, PaginationMetaData)> GetAllProductsAsync
            (string? searchQuery, int pageNumber, int pageSize)
        {
            IQueryable<ProductEntity> collection = db.Products as IQueryable<ProductEntity>;

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

        public async Task<ProductEntity?> GetProductByIdAsync(int id)
        {
            return await db.Products.FindAsync(id);
        }

        public async Task<bool> IsProductExists(int id)
        {
            ProductEntity? product = await GetProductByIdAsync(id);
            return product != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
