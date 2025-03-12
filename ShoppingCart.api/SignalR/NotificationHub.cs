using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ShoppingCart.api.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        //TODO: in the production version this must be done to Redis rather than to in memory dictionary for 
        //scalability
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

        public override Task OnConnectedAsync()
        {
            string? email = Context.User?.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(email))
            {
                UserConnections[email] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string? email = Context.User?.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(email))
            {
                UserConnections.TryRemove(email, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionIdByEmail(string email)
        {
            UserConnections.TryGetValue(email, out string? connectionId);
            return connectionId;
        }
    }
}
