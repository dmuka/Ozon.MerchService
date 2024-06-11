using System.Reflection;

namespace Ozon.MerchandizeService.Configuration.Constants;

/// <summary>
/// Contain configuration names constants
/// </summary>
public static class Names
{
    public const string DefaultApplicationName = "Ozon.MerchService";
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