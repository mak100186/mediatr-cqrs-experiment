namespace Maxx.Domain.DomainEvents;

using Primitives;

public abstract record DomainEvent(Guid Id) : IDomainEvent;