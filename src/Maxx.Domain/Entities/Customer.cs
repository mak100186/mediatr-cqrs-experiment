namespace Maxx.Domain.Entities;

using DomainEvents;

using ValueObjects;

using Primitives;

public class Customer : AggregateRoot, IAuditableEntity
{
    private Customer(Guid id, Email email, FirstName firstName, LastName lastName, VirtualWalletAccount virtualWalletAccount)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        VirtualWalletAccount = virtualWalletAccount;
    }

    private Customer()
    {
    }

    public Email Email { get; set; }

    public FirstName FirstName { get; set; }

    public LastName LastName { get; set; }

    public VirtualWalletAccount VirtualWalletAccount { get; set; }

    public List<Reward> Rewards { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public static Customer Create(
        Guid id,
        Email email,
        FirstName firstName,
        LastName lastName,
        VirtualWalletAccount virtualWalletAccount)
    {
        var customer = new Customer(
            id,
            email,
            firstName,
            lastName,
            virtualWalletAccount);

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(Guid.NewGuid(), customer.Id));

        return customer;
    }
}