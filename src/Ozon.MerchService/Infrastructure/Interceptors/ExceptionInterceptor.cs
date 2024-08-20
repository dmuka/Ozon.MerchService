using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.MerchService.Infrastructure.Interceptors;

/// <summary>
/// Grpc server exception interceptor
/// </summary>
public class ExceptionInterceptor : Interceptor
{
    /// <summary>
    /// Handler to log unary grpc server requests
    /// </summary>
    /// <param name="request">Request instance</param>
    /// <param name="context">Context instance for a server side call</param>
    /// <param name="continuation">Next server side handler for unary call</param>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <returns>Response instance</returns>
    /// <exception cref="RpcException">Rpc exception</exception>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
             
            throw new RpcException(new Status(StatusCode.Internal, exception.ToString()));
        }
    }
}