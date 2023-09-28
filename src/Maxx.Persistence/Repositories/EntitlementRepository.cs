namespace Maxx.Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;

internal sealed class EntitlementRepository : IEntitlementRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EntitlementRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public void Add(Entitlement entitlement)
    {
        this._dbContext.Set<Entitlement>().Add(entitlement);
    }
}