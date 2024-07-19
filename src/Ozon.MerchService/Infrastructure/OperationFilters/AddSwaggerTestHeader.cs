using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ozon.MerchService.Infrastructure.OperationFilters;

/// <summary>
/// Add test header in the Swagger UI
/// </summary>
public class AddSwaggerTestHeader : IOperationFilter
{
    /// <summary>
    /// Add test header to OpenAPI operation  
    /// </summary>
    /// <param name="operation">OpenAPI operation object</param>
    /// <param name="context">Operation filter context object</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add((new OpenApiParameter
        {
            In = ParameterLocation.Header,
            Name = "test-header",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        }));
    }
}