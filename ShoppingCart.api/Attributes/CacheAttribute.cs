using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ShoppingCart.api.Controllers;
using ShoppingCart.data.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace ShoppingCart.api.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class CacheAttribute(int timeToLiveSeconds) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                try
                {
                    JsonDocument document = JsonDocument.Parse(cachedResponse);
                    JsonElement root = document.RootElement;

                    // Extract content and pagination properties
                    if (root.TryGetProperty("content", out JsonElement contentElement) &&
                        root.TryGetProperty("pagination", out JsonElement paginationElement))
                    {
                        string content = contentElement.GetString() ?? string.Empty;
                        string pagination = paginationElement.GetString() ?? string.Empty;

                        // Use these values
                        context.HttpContext.Response.Headers.Append("X-Pagination", pagination);
                        var contentResult = new ContentResult
                        {
                            Content = content,
                            ContentType = "application/json",
                            StatusCode = 200
                        };
                        context.Result = contentResult;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
          
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value != null)
                {
                    // Get the pagination header if it exists
                    string? paginationHeader = string.Empty;
                    if (context.HttpContext.Response.Headers.TryGetValue("X-Pagination", out var headerValues))
                    {
                        paginationHeader = headerValues;
                    }

                    // Create a structured cached response
                    var responseToCache = new CachedResponseObject
                    {
                        Content = okObjectResult.Value,
                        Pagination = paginationHeader
                    };

                    // Cache the response
                    await cacheService.CacheResponseAsync(
                        cacheKey,
                        responseToCache,
                        TimeSpan.FromSeconds(timeToLiveSeconds)
                    );
                }
            }
        }

        public class CachedResponseObject
        {
            public object? Content { get; set; }
            public string? Pagination { get; set; } = string.Empty;
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach(var(key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
