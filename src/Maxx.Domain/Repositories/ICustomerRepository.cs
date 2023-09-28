namespace Maxx.Domain.Repositories;

using Entities;

public interface ICustomerRepository
{
    void Add(Customer customer);
    void Update(Customer customer);
}