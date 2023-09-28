namespace Maxx.Domain.ValueObjects;

using Errors;

using Primitives;

using Shared;

public sealed class Email : ValueObject
{
    private Email(string value) => this.Value = value;

    public string Value { get; }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(DomainErrors.Email.Empty);
        }

        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);
        }

        return new Email(email);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return this.Value;
    }
}