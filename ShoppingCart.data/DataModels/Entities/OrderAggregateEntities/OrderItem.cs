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
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
