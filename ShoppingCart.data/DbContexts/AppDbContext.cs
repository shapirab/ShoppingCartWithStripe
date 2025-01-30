using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductBrandEntity> ProductBrands { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes { get; set; }

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
    }
}
