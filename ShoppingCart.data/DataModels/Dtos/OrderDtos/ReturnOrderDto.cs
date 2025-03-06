using ShoppingCart.data.DataModels.Entities.OrderAggregateEntities;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Dtos.OrderDtos
{
    public class ReturnOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string CustomerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public required DeliveryMethodEntity DeliveryMethod { get; set; }
        public required PaymentSummary PaymentSummary { get; set; }
        public required List<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public required string OrderStatus { get; set; }
        public required string PaymentIntentId { get; set; }
    }
}
