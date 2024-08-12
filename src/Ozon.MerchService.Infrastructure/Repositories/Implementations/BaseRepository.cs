using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts.Attributes;
using Ozon.MerchService.Domain.Models;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Attributes;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public abstract class BaseRepository() 
{
    internal string GetTableName<T>()
    {
        var type = typeof(T);
        
        return GetTableNameByAttribute(type);
    }
    
    internal string GetTableName(Type type)
    {
        return GetTableNameByAttribute(type);
    }

    private string GetTableNameByAttribute(Type type)
    {
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        
        return tableAttribute != null ? tableAttribute.Name : type.Name;
    }

    internal long GetKnownDtoKeyColumnValue<TDto>(TDto dto)
    {
        var keyColumnValue = dto switch
        {
            MerchPackRequestDto dt => dt.Id,
            ClothingSizeDto dt => dt.Id,
            EmployeeDto dt => dt.Id,
            MerchItemDto dt => dt.ItemTypeId,
            _ => throw new InvalidOperationException($"Unsupported DTO type: {dto?.GetType().Name}")
        };

        return keyColumnValue;
    }

    internal Type GetDtoTypeByEntityType<T>() where T : Entity
    {
        var entityType = typeof(T);

        if (EntityToDtoMap.TryGetValue(entityType, out var dtoType))
        {
            return dtoType;
        }

        throw new InvalidOperationException($"No DTO type mapping found for entity type {entityType.Name}");
    }
    
    private static readonly Dictionary<Type, Type> EntityToDtoMap = new Dictionary<Type, Type>
    {
        { typeof(MerchPackRequest), typeof(MerchPackRequestDto) },
        { typeof(ClothingSize), typeof(ClothingSizeDto) },
        { typeof(Employee), typeof(EmployeeDto) },
        { typeof(MerchItem), typeof(MerchItemDto) },
        { typeof(MerchPack), typeof(MerchPackDto) }
    };

    internal static string? GetKeyColumnName(Type type)
    {
        var properties = type.GetProperties();

        return GetKeyColumnName(properties);
    }

    internal static string? GetKeyColumnName<T>()
    {
        var properties = typeof(T).GetProperties();

        return GetKeyColumnName(properties);
    }

    private static string? GetKeyColumnName(PropertyInfo[] properties)
    {
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

    internal string GetColumns<T>(bool excludeKey = false)
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

    internal string GetColumnsNames<T>(bool excludeKey = false)
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

    internal string GetPropertyValues<T>(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || 
                        !p.IsDefined(typeof(KeyAttribute)) && 
                        !p.IsDefined(typeof(NotMappedAttribute)));

        var values = string.Join(", ", properties.Select(p => p.IsDefined(typeof(JsonAttribute)) ? $"CAST(@{p.Name} as json)" : $"@{p.Name}"));

        return values;
    }

    internal IEnumerable<PropertyInfo> GetProperties<T>(bool excludeKey = false)
    {
        var properties = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }

    internal IEnumerable<PropertyInfo> GetProperties(Type type, bool excludeKey = false)
    {
        var properties = type.GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }

    internal string? GetKeyPropertyValue<T>()
    {
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

        if (properties.Count != 0)
            return properties?.FirstOrDefault()?.Name ?? null;

        return null;
    }

    internal string? GetKeyPropertyValue(Type type)
    {
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

        if (properties.Count != 0)
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