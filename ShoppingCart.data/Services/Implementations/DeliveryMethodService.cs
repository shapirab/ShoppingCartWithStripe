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
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private readonly AppDbContext db;

        public DeliveryMethodService(AppDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task AddDeliveryMethodAsync(DeliveryMethodEntity deliveryMethod)
        {
            await db.DeliveryMethods.AddAsync(deliveryMethod);
        }

        public async Task DeleteDeliveryMethodAsync(int id)
        {
            DeliveryMethodEntity? deliveryMethodEntity = await db.DeliveryMethods.FindAsync(id);
            if (deliveryMethodEntity != null)
            {
                db.DeliveryMethods.Remove(deliveryMethodEntity);
            }
            ;
        }

        public async Task<IEnumerable<DeliveryMethodEntity>> GetAllDeliveryMethodsAsync()
        {
            return await db.DeliveryMethods.OrderBy(delMethod => delMethod.ShortName).ToListAsync();
        }

        public async Task<(IEnumerable<DeliveryMethodEntity>, PaginationMetaData)> GetAllDeliveryMethodsAsync
            (string? searchQuery, string? sort, int pageNumber, int pageSize)
        {
            IQueryable<DeliveryMethodEntity> collection = db.DeliveryMethods as IQueryable<DeliveryMethodEntity>;
            collection = collection.OrderBy(delMethod => delMethod.ShortName);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(delMethod => delMethod.ShortName.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceASC":
                        collection = collection.OrderBy(delMethod => delMethod.Price);
                        break;
                    case "priceDESC":
                        collection = collection.OrderByDescending(delMethod => delMethod.Price);
                        break;
                    default:
                        collection = collection.OrderBy(delMethod => delMethod.ShortName);
                        break;
                }
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<DeliveryMethodEntity?> GetDeliveryMethodByIdAsync(int id)
        {
            return await db.DeliveryMethods.FindAsync(id);
        }

        public async Task<bool> IsDeliveryMethodExists(int id)
        {
            DeliveryMethodEntity? deliveryMethodEntity = await GetDeliveryMethodByIdAsync(id);
            return deliveryMethodEntity != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
