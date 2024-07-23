using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Ozon.MerchService.Domain.DataContracts.Attributes;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public abstract class BaseRepository<T>() 
{
    internal string GetTableName()
    {
        var type = typeof(T);
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
            return tableAttribute.Name;

        return type.Name;
    }

    internal static string? GetKeyColumnName()
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

            if (keyAttributes != null && keyAttributes.Length > 0)
            {
                var columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                if (columnAttributes != null && columnAttributes.Length > 0)
                {
                    var columnAttribute = (ColumnAttribute)columnAttributes[0];
                    
                    return columnAttribute?.Name ?? "";
                }
                else
                {
                    return property.Name;
                }
            }
        }

        return null;
    }

    internal string GetColumns(bool excludeKey = false)
    {
        var type = typeof(T);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute != null ? columnAttribute.Name : p.Name;
            }));

        return columns;
    }

    internal string GetColumnsNames(bool excludeKey = false)
    {
        var type = typeof(T);
        var columnsNames = string.Join(", ", type.GetProperties()
            .Where(p => (!excludeKey || !p.IsDefined(typeof(KeyAttribute))) && !p.IsDefined(typeof(ColumnExcludeAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute != null ? $"{columnAttribute.Name} AS {p.Name}" : p.Name;
            }));

        return columnsNames;
    }

    internal string GetPropertyValues(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        return values;
    }

    internal IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }

    internal string? GetKeyPropertyValue()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

        if (properties.Any())
            return properties?.FirstOrDefault()?.Name ?? null;

        return null;
    }
}