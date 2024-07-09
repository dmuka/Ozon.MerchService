using MediatR;

namespace Ozon.MerchService.Domain.Models;

public abstract class Entity<TKey>
    where TKey : IEquatable<TKey>
{
    private int? _requestedHashCode;

    public TKey Id { get; protected set; }

    private readonly List<INotification> _domainEvents = [];

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();

    private bool IsTransient() => Id.Equals(default);

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TKey> entity) return false;

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
    public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
    {
        return !(left == right);
    }
}