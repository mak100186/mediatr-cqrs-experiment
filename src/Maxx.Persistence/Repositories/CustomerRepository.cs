namespace Maxx.Persistence.Repositories;

using Domain.Repositories;

using Domain.Entities;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public void Add(Customer customer)
    {
        _dbContext.Set<Customer>().Add(customer);
    }

    public void Update(Customer customer)
    {
        this._dbContext.Set<Customer>().Update(customer);
    }
}