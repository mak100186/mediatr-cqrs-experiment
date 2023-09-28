namespace Maxx.Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;

internal sealed class RewardRepository : IRewardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RewardRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public void Add(Reward reward)
    {
        this._dbContext.Set<Reward>().Add(reward);
    }
}