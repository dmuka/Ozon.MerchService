using OpenTelemetry.Trace;

namespace Ozon.MerchService.Infrastructure.Helpers;

/// <summary>
/// Tracing helper
/// </summary>
public static class Tracing
{
    /// <summary>
    /// Open telemetry tracer start span logic
    /// </summary>
    /// <param name="tracer">Open telemetry tracer instance</param>
    /// <param name="spanName">Name of span</param>
    /// <param name="kind">Kind of span</param>
    /// <param name="attributes">Span attributes collection</param>
    /// <returns>IDisposable instance of span</returns>
    public static IDisposable StartSpan(
        Tracer tracer, 
        string spanName, 
        SpanKind kind, 
        IDictionary<string, object>? attributes = null)
    {
        var span = tracer.StartActiveSpan(spanName, kind);

        if (attributes is null) return span;
        
        foreach (var attribute in attributes)
        {
            span.SetAttribute(attribute.Key, attribute.Value.ToString());
        }

        return span;
    }
}