namespace Maxx.Domain.Errors;

using Shared;

public static class DomainErrors
{
    public static class Customer
    {
        public static readonly Error EmailAlreadyInUse = new(
            "Customer.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Customer.NotFound",
            $"The customer with the identifier {id} was not found.");
    }

    public static class VirtualWalletAccount
    {
        public static readonly Error Empty = new(
            "VirtualWalletAccount.Empty",
            "Virtual wallet account is empty");

        public static readonly Error InvalidFormat = new(
            "VirtualWalletAccount.InvalidFormat",
            "Virtual wallet account format is invalid");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "VirtualWalletAccount.NotFound",
            $"Virtual wallet account with the identifier {id} was not found.");
    }

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }
}