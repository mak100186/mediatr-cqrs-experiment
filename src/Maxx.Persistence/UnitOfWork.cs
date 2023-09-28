using Maxx.Domain.Repositories;

namespace Maxx.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext) => this._dbContext = dbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        this._dbContext.SaveChangesAsync(cancellationToken);
}