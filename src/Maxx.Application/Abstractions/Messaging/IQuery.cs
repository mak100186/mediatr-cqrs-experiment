using Maxx.Domain.Shared;

using MediatR;

namespace Maxx.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
