using FluentValidation;
using MediatR;
using Ozon.MerchService.Domain.Exceptions;
using Ozon.MerchService.Infrastructure.Extensions;

namespace Ozon.MerchService.Infrastructure.Mediatr;

/// <summary>
/// Log validation logic in mediatr pipeline
/// </summary>
/// <param name="logger">Logger instance</param>
/// <param name="validators">Collection of current validators</param>
/// <typeparam name="TRequest">Type of request</typeparam>
/// <typeparam name="TResponse">Type of response</typeparam>
public class ValidatorBehavior<TRequest, TResponse>(
    ILogger<ValidatorBehavior<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handle logic to log validation logic in mediatr pipeline
    /// </summary>
    /// <param name="request">Request instance</param>
    /// <param name="next">Next handler in mediatr pipeline</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="MerchServiceDomainException"></exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        logger.LogInformation("Validating command {CommandType}", typeName);

        var failures = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Count != 0)
        {
            logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new MerchServiceDomainException(
                $"Command validation errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
        }

        return await next();
    }
}