using Maxx.Domain.Primitives;

using MediatR;

namespace Maxx.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
