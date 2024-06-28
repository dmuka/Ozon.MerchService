using System.Reflection;

namespace Ozon.MerchService.Configuration.Constants;

/// <summary>
/// Contain configuration names constants
/// </summary>
public static class Names
{
    /// <summary>
    /// Default application name
    /// </summary>
    public const string DefaultApplicationName = "Ozon.MerchService";
    
    /// <summary>
    /// Swagger doc version
    /// </summary>
    public const string SwaggerDocVersion = "Version 1";

    /// <summary>
    /// Get application name or it's default value
    /// </summary>
    /// <returns>Application name</returns>
    public static string GetApplicationName()
    {
        return Assembly.GetExecutingAssembly().GetName().Name ?? DefaultApplicationName;
    }
}