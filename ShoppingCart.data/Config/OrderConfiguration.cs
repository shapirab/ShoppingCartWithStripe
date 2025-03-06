using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, navBuilder => navBuilder.WithOwner());
            builder.OwnsOne(order => order.PaymentSummary, navBuilder => navBuilder.WithOwner());
            builder.Property(order => order.OrderStatus).HasConversion(
                orderStatus => orderStatus.ToString(),
                orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
            );
            builder.HasMany(order => order.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(order => order.OrderDate).HasConversion(
                date => date.ToUniversalTime(),
                date => DateTime.SpecifyKind(date, DateTimeKind.Utc)
            );
        }
    }
}
