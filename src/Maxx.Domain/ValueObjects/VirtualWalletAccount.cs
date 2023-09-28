namespace Maxx.Domain.ValueObjects;

using Errors;

using Primitives;

using Shared;

public sealed class VirtualWalletAccount : ValueObject
{
    private VirtualWalletAccount(Guid accountId) => this.AccountId = accountId;

    public Guid AccountId { get; }

    public static Result<VirtualWalletAccount> Create(string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            return Result.Failure<VirtualWalletAccount>(DomainErrors.VirtualWalletAccount.Empty);
        }

        if (Guid.TryParse(accountId, out var parsedAccountId))
        {
            return Result.Failure<VirtualWalletAccount>(DomainErrors.VirtualWalletAccount.InvalidFormat);
        }

        return new VirtualWalletAccount(parsedAccountId);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return this.AccountId;
    }
}