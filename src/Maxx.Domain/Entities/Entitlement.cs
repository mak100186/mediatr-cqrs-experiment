namespace Maxx.Domain.Entities;

using DomainEvents;

using Primitives;

public class Entitlement : AggregateRoot, IAuditableEntity
{
    private Entitlement()
    {

    }

    private Entitlement(Guid rewardId, Guid customerId) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        RewardId = rewardId;
    }

    public Guid RewardId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Reward Reward { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public static Entitlement Create(Guid rewardId, Guid customerId)
    {
        var entitlement = new Entitlement(rewardId, customerId);

        entitlement.RaiseDomainEvent(new EntitlementCreatedDomainEvent(Guid.NewGuid(), entitlement.Id));

        return entitlement;
    }
}
