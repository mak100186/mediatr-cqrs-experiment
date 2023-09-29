using Maxx.Domain.Shared;

using MediatR;

namespace Maxx.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
