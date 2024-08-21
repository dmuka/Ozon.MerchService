namespace Ozon.MerchService.Infrastructure.Middlewares;

/// <summary>
/// Middleware for live response
/// </summary>
public class LiveMiddleware()
{
    /// <summary>
    /// Return 200 Ok status code in the response
    /// </summary>
    /// <param name="context">Http context object</param>
    public async Task InvokeAsync(HttpContext context)
    {
        await Task.FromResult(context.Response.StatusCode = StatusCodes.Status200OK);
    }
}