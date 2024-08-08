using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Ozon.MerchService.Domain.DataContracts.Attributes;

namespace Ozon.MerchService.Domain.Models;

public abstract class Entity : BaseEntity
{
    private int? _requestedHashCode;

    private readonly List<INotification> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();

    private bool IsTransient() => Id.Equals(default);

    public override bool Equals(object obj)
    {
        if (obj is not Entity entity) return false;

        if (ReferenceEquals(this, entity)) return true;

        if (GetType() != entity.GetType()) return false;

        if (entity.IsTransient() || IsTransient())
            return false;
        else
            return entity.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            _requestedHashCode ??= HashCode.Combine(Id, 31);

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();

    }
    public static bool operator ==(Entity left, Entity right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}