namespace Maxx.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(Guid id)
        : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => this._domainEvents.ToList();

    public void ClearDomainEvents() => this._domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        this._domainEvents.Add(domainEvent);
}