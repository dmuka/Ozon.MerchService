using OpenTelemetry.Trace;

namespace Ozon.MerchService.Infrastructure.Helpers;

public static class Tracing
{
    public static IDisposable StartSpan(
        Tracer tracer, 
        string spanName, 
        SpanKind kind, 
        IDictionary<string, object> attributes = null)
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