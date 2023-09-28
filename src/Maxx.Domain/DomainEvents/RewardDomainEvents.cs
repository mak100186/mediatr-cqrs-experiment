using Maxx.Domain.DomainEvents;

public sealed record RewardCreatedDomainEvent(Guid Id, Guid MemberId) : DomainEvent(Id);
