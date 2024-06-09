using Ozon.MerchandizeService.Configuration.Constants;

namespace Ozon.MerchandizeService.Configuration.Middlewares;

/// <summary>
/// Middleware for live response
/// </summary>
public class LiveMiddleware
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    public LiveMiddleware(RequestDelegate next) {}
    
    /// <summary>
    /// Return 200 Ok status code in the response
    /// </summary>
    /// <param name="context">Http context object</param>
    public async Task InvokeAsync(HttpContext context)
    {
        await Task.FromResult(context.Response.StatusCode = StatusCodes.Status200OK);
    }
}