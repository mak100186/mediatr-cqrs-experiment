using Maxx.Domain.Primitives;
using Maxx.Persistence;
using Maxx.Persistence.Outbox;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Quartz;

namespace Maxx.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }
}
