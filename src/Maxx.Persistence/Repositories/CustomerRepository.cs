using Maxx.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Maxx.Persistence.Repositories;

using System;

using Domain.Entities;
using Domain.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Customer customer)
    {
        _dbContext.Set<Customer>().Add(customer);
    }

    public void Update(Customer customer)
    {
        _dbContext.Set<Customer>().Update(customer);
    }

    public async Task<bool> IsEmailUniqueAsync(Email emailResultValue, CancellationToken cancellationToken) =>
         !await _dbContext
             .Set<Customer>()
            .AnyAsync(x=> x.Email.Equals(emailResultValue), cancellationToken);

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _dbContext
            .Set<Customer>()
            .FirstOrDefaultAsync(member => member.Id == id, cancellationToken);
}
