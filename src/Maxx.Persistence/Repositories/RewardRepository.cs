namespace Maxx.Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;

internal sealed class RewardRepository : IRewardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RewardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Reward reward)
    {
        _dbContext.Set<Reward>().Add(reward);
    }
}
