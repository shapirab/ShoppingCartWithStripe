using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.data.DataModels.Models.OrderAggregate;

namespace ShoppingCart.data.DataModels.Entities.OrderAggregateEntities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public required string CustomerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; } = null!;
        public DeliveryMethodEntity DeliveryMethod { get; set; } = null!;
        public PaymentSummary PaymentSummary { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal Subtotal { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.PENDING;
        public required string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
