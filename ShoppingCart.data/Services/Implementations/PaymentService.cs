using Microsoft.Extensions.Configuration;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService cartService;
        private readonly IProductService productService;
        private readonly IDeliveryMethodService deliveryMethodService;

        public PaymentService(IConfiguration config, ICartService cartService,
        IProductService productService, IDeliveryMethodService deliveryMethodService)
        {
            this.cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.deliveryMethodService = deliveryMethodService ?? 
                throw new ArgumentNullException(nameof(deliveryMethodService));

            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        }

        public async Task<ShoppingCartModel?> CreateOrUpdatePaymentIntent(string cartId)
        {
            ShoppingCartModel? cart = await cartService.GetShoppingCartAsync(cartId);
            if (cart == null) return null;

            decimal shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                DeliveryMethodEntity? deliveryMethod =
                    await deliveryMethodService.GetDeliveryMethodByIdAsync((int)cart.DeliveryMethodId);
                if (deliveryMethod == null) return null;

                shippingPrice = deliveryMethod.Price;
            }

            foreach (CartItem item in cart.Items)
            {
                ProductEntity? product = await productService.GetProductByIdAsync(item.ProductId);
                if (product == null) return null;
                if (product.Price != item.Price)
                {
                    item.Price = product.Price;
                }
            }

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent? paymentIntent = null;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(item => item.Quantity * (item.Price * 100)) + 
                                (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);

                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                PaymentIntentUpdateOptions options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100
                };
                await paymentIntentService.UpdateAsync(cart.PaymentIntentId, options);
            }
            await cartService.AddOrUpdateShoppingCartAsync(cart);
            return cart;
        }

        public async Task<string> RefundPayment(string paymentIndentId)
        {
            RefundCreateOptions refundOptions = new RefundCreateOptions
            {
                PaymentIntent = paymentIndentId
            };

            RefundService refundService = new RefundService();
            var results = await refundService.CreateAsync(refundOptions);
            return results.Status;
        }
    }
}
