using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Models.OrderAggregate
{
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string ImageUrl { get; set; }
    }
}
