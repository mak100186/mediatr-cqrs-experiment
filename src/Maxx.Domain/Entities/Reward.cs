namespace Maxx.Domain.Entities;

using Enums;

using Primitives;

public class Reward : AggregateRoot, IAuditableEntity
{
    private Reward()
    {
    }

    private Reward(
        Guid id,
        RewardType type,
        MetaData metaData)
        : base(id)
    {
        Type = type;
        MetaData = metaData;
    }

    public RewardType Type { get; set; }

    public MetaData MetaData { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public List<Customer> Customers { get; set; }

    public static Reward Create(
        Guid id,
        RewardType type,
        MetaData metaData)
    {
        var reward = new Reward(id, type, metaData);

        reward.RaiseDomainEvent(new RewardCreatedDomainEvent(Guid.NewGuid(), id));

        return reward;
    }
}

public abstract class MetaData : ValueObject
{
    public Type Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class FreeBetMetaData : MetaData
{
    public FreeBetMetaData()
    {
        Type = typeof(FreeBetMetaData);
    }

    public decimal Amount { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Type;
        yield return StartDate;
        yield return ExpiryDate;
    }
}

public class FreeBetBalanceMetaData : MetaData
{
    public FreeBetBalanceMetaData()
    {
        Type = typeof(FreeBetBalanceMetaData);
    }

    public decimal Amount { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Type;
        yield return StartDate;
        yield return ExpiryDate;
    }
}

public class UniBoostMetaData : MetaData
{
    public UniBoostMetaData()
    {
        Type = typeof(UniBoostMetaData);
    }

    public decimal Boost { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Boost;
        yield return Type;
        yield return StartDate;
        yield return ExpiryDate;
    }
}

public class UniBoostReloadMetaData : UniBoostMetaData
{
    public UniBoostReloadMetaData()
    {
        Type = typeof(UniBoostReloadMetaData);
    }

    public int ReloadCount { get; set; }
    public TimeSpan ReloadInterval { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return ReloadCount;
        yield return ReloadInterval;
        yield return Type;
        yield return StartDate;
        yield return ExpiryDate;
    }
}

public class ProfitBoostMetaData : MetaData
{
    public ProfitBoostMetaData()
    {
        Type = typeof(ProfitBoostMetaData);
    }

    public IDictionary<int, decimal> LegTable { get; set; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return LegTable;
        yield return Type;
        yield return StartDate;
        yield return ExpiryDate;
    }
}
