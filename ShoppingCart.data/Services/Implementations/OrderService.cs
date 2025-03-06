using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using ShoppingCart.data.DbContexts;
using ShoppingCart.data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext db;

        public OrderService(AppDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddOrderAsync(Order order)
        {
            await db.Orders.AddAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            Order? order = await db.Orders.FindAsync(id);
            if(order != null)
            {
                db.Orders.Remove(order);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await db.Orders.OrderBy(order => order.OrderDate)
                .Include(order => order.DeliveryMethod)
                .Include(order => order.OrderItems)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Order>, PaginationMetaData)> GetAllOrdersAsync
            (string? searchQuery, DateTime? orderDate, OrderStatus? orderStatus, 
            int pageNumber, int pageSize)
        {
            IQueryable<Order> collection = db.Orders as IQueryable<Order>;
            collection = collection.OrderBy(order => order.OrderDate);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(order => order.CustomerEmail.Contains(searchQuery));
            }

            if (orderDate != null)
            {
                collection = collection.Where(order => order.OrderDate == orderDate);
            }

            if (orderStatus != null)
            {
                collection = collection.Where(order => order.OrderStatus == orderStatus);
            }

            int totalItemCount = await collection.CountAsync();

            PaginationMetaData paginationMetadata = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Include(order => order.DeliveryMethod)
                .Include(order => order.OrderItems)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string email)
        {
            return await db.Orders.Where(order => order.CustomerEmail == email)
                .Include(order => order.DeliveryMethod)
                .Include(order => order.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await db.Orders.Where(order => order.Id == id)
                .Include(order => order.DeliveryMethod)
                .Include(order => order.OrderItems)
                .FirstOrDefaultAsync();
        }


        public async Task<bool> IsOrderExists(int id)
        {
            Order? order = await GetOrderByIdAsync(id);
            return order != null;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
