namespace Maxx.Domain.DomainEvents;

public sealed record EntitlementCreatedDomainEvent(Guid Id, Guid MemberId) : DomainEvent(Id);