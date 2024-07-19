using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ozon.MerchService.Infrastructure.ExceptionsFilters;

/// <summary>
/// Global exception filter
/// </summary>
public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    /// <summary>
    /// Add information about unhandled exception
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        var result = new
        {
            ExceptionName = context.Exception.GetType().ToString(),
            StackTrace = context.Exception.StackTrace
        };

        var jsonResult = new JsonResult(result)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Result = jsonResult;
    }
}