using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.MerchService.Infrastructure.Interceptors;

/// <summary>
/// Grpc server logging interceptor
/// </summary>
/// <param name="logger">Logger instance</param>
public class LoggingInterceptor(ILogger<LoggingInterceptor> logger) : Interceptor
{
    private readonly JsonSerializerOptions _defaultSerializationOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Handler to log unary grpc server requests
    /// </summary>
    /// <param name="request">Request instance</param>
    /// <param name="context">Context instance for a server side call</param>
    /// <param name="continuation">Next server side handler for unary call</param>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <returns>Response instance</returns>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var method = context.Method;
            
            logger.LogInformation("gRPC request {method}:", method);
            
            var requestJson = JsonSerializer.Serialize(request, _defaultSerializationOptions);
            
            logger.LogInformation("Json: {requestJson}", requestJson);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Couldn't log gRPC request");
        }

        var response = await base.UnaryServerHandler(request, context, continuation);

        try
        {
            var responseJson = JsonSerializer.Serialize(response, _defaultSerializationOptions);
            logger.LogInformation(responseJson);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Couldn't log gRPC response");
        }

        return response;
    }
}