using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DataModels.Entities;
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
