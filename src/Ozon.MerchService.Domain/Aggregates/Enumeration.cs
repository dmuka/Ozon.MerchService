using System.Reflection;

namespace Ozon.MerchService.Domain.Models;

/// <summary>
/// Represents enumeration abstract class
/// Source: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
/// </summary>
public abstract class Enumeration(int id, string name) : IComparable
{
    public string Name { get; } = name;

    public int Id { get; } = id;

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration toEqual)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(toEqual.Id);

        return typeMatches && valueMatches;
    }

    protected bool Equals(Enumeration toEqual)
    {
        return Name == toEqual.Name && Id == toEqual.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Id);
    }

    public int CompareTo(object toCompare) => Id.CompareTo(((Enumeration)toCompare).Id);
}