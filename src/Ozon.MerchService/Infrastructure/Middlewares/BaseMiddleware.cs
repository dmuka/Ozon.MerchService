using System.Text;

namespace Ozon.MerchService.Infrastructure.Middlewares;

public class BaseMiddleware
{
    protected string GetRouteHeadersLogMessage(string route, IHeaderDictionary headers)
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