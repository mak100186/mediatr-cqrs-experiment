namespace Maxx.Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;

internal sealed class EntitlementRepository : IEntitlementRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EntitlementRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Entitlement entitlement)
    {
        _dbContext.Set<Entitlement>().Add(entitlement);
    }
}
