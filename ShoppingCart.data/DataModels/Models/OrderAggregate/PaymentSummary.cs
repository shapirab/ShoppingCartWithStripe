using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Models.OrderAggregate
{
    public class PaymentSummary
    {
        public int LastFourDigits { get; set; }
        public required string CreditCartType { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
    }
}
