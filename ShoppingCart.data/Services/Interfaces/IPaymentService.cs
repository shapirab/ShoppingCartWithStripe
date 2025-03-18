using ShoppingCart.data.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ShoppingCartModel?> CreateOrUpdatePaymentIntent(string cartId);
        Task<string> RefundPayment(string paymentIndentId);
    }
}
