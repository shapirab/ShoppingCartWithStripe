using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.data.Services.Interfaces;

namespace ShoppingCart.api.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class InvalidateCache(string pattern) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultsContext = next();

            if(resultsContext.Exception == null)
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                await cacheService.RemoveChacheByPatternAsync(pattern);
            }
        }
    }
}
