namespace Maxx.Domain.DomainEvents;

public sealed record CustomerCreatedDomainEvent(Guid Id, Guid CustomerId) : DomainEvent(Id);
public sealed record CustomerUpdatedDomainEvent(Guid Id, Guid CustomerId) : DomainEvent(Id);
