using Microsoft.EntityFrameworkCore;
using ShoppingCart.data.DbContexts;
using ShoppingCart.data.Services.Implementations;
using ShoppingCart.data.Services.Interfaces;
using StackExchange.Redis;

namespace ShoppingCart.api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //services.AddDbContext<AppDbContext>(
            //    dbContextOptions => dbContextOptions.UseSqlServer(
            //        config.GetConnectionString("ConnectionStrings:ShoppingCartDB")));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IProductBrandService, ProductBrandService>();
            services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton<ICartService, ShoppingCartReditService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            return services;
        }
    }
}
