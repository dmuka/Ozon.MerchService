namespace Ozon.MerchService.Infrastructure.Extensions;

/// <summary>
/// Generic type extensions
/// </summary>
public static class GenericTypeExtensions
{
    /// <summary>
    /// Get name of generic type
    /// </summary>
    /// <param name="type">Type instance</param>
    /// <returns>Name of type</returns>
    public static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    /// <summary>
    /// Get name of generic type
    /// </summary>
    /// <param name="object">Object instance</param>
    /// <returns>Name of type</returns>
    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}