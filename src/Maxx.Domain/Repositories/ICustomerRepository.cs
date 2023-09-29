using Maxx.Domain.ValueObjects;

namespace Maxx.Domain.Repositories;

using Entities;

public interface ICustomerRepository
{
    void Add(Customer customer);
    void Update(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(Email emailResultValue, CancellationToken cancellationToken = default);
}
