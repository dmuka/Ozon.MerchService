using MediatR;

namespace Ozon.MerchService.Infrastructure.Mediatr;

public class ScopedBehavior<TRequest, TResponse>(IServiceScopeFactory serviceScopeFactory)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        return await next();
    }
}