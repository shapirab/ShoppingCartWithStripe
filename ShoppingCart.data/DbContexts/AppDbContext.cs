using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.Config;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DbContexts
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductBrandEntity> ProductBrands { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes { get; set; }

        public DbSet<DeliveryMethodEntity> DeliveryMethods { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Instead of this we can use a centrelized location read from the line below
            //builder.Entity<Order>().OwnsOne(x => x.ShippingAddress, o => o.WithOwner())
            //builder.Entity<Order>().HasMany(order => order.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
        }

        //public DbSet<Address> Addresses { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ProductEntity>()
        //       .HasOne(brand => brand.ProductBrand)
        //       .WithMany()
        //       .HasForeignKey(prod => prod.ProductBrandId);

        //    modelBuilder.Entity<ProductEntity>()
        //      .HasOne(type => type.ProductType)
        //      .WithMany()
        //      .HasForeignKey(prod => prod.ProductTypeId);
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Address>().HasNoKey();
        //}
    }
}
