using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts.Attributes;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public abstract class BaseRepository<T>() 
{
    internal string GetTableName()
    {
        var type = typeof(T);
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        return tableAttribute != null ? tableAttribute.Name : type.Name;
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
            .Where(p => !excludeKey || 
                        !p.IsDefined(typeof(KeyAttribute)) && 
                        !p.IsDefined(typeof(NotMappedAttribute)))
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
            .Where(p => !excludeKey || 
                        !p.IsDefined(typeof(KeyAttribute)) && 
                        !p.IsDefined(typeof(NotMappedAttribute)));

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

    internal static MerchType GetMerchPackType(int merchPackId)
    {
        return merchPackId switch
        {
            (int)MerchType.WelcomePack => MerchType.WelcomePack,
            (int)MerchType.ProbationPeriodEndingPack => MerchType.ProbationPeriodEndingPack,
            (int)MerchType.ConferenceListenerPack => MerchType.ConferenceListenerPack,
            (int)MerchType.ConferenceSpeakerPack => MerchType.ConferenceSpeakerPack,
            (int)MerchType.VeteranPack => MerchType.VeteranPack,
            _ => throw new ArgumentException("Unknown merch type value")
        };
    }

    internal static ClothingSize GetClothingSize(int clothingSizeId)
    {
        return clothingSizeId switch
        {
            (int)ClothingSize.XS => ClothingSize.XS,
            (int)ClothingSize.S => ClothingSize.S,
            (int)ClothingSize.M => ClothingSize.M,
            (int)ClothingSize.L => ClothingSize.L,
            (int)ClothingSize.XL => ClothingSize.XL,
            (int)ClothingSize.XXL => ClothingSize.XXL,
            _ => throw new ArgumentException("Unknown clothing size value")
                                    
        };
    }
}