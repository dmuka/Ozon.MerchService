using System.Text;

namespace Ozon.MerchService.Infrastructure.Middlewares;

/// <summary>
/// Base middleware class
/// </summary>
public abstract class BaseMiddleware
{
    /// <summary>
    /// Get headers for route
    /// </summary>
    /// <param name="route">Route value</param>
    /// <param name="headers">Headers dictionary</param>
    /// <returns></returns>
    protected static string GetRouteHeadersLogMessage(string route, IHeaderDictionary headers)
    {
        var message = new StringBuilder();
        
        if (headers.Count > 0)
        {
            message.AppendLine($"Route: {route}");
            message.AppendLine("Headers:");
            
            foreach (var header in headers)
            {
                message.AppendLine($"Key: {header.Key}, value: {header.Value}.");
            }
        }
        else
        {
            message.Append($"Route: {route}, no headers.");
        }

        return message.ToString();
    }
}