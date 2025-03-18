using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            string adminId = "b8a9d9a1-d029-4f21-a9cd-b0df9e0f3abe";
            string customerId = "edc3c74c-e90e-45d7-a7c4-4c3c7c4b1234";
            builder.HasData(
                new IdentityRole
                {
                    Id = adminId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = customerId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                });
        }
    }
}
