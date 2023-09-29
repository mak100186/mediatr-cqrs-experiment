using Carter;

using FluentValidation;

using Maxx.Application.Abstractions.Endpoints;
using Maxx.Application.Abstractions.Messaging;
using Maxx.Domain.Entities;
using Maxx.Domain.Errors;
using Maxx.Domain.Repositories;
using Maxx.Domain.Shared;
using Maxx.Domain.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Maxx.Application.Handlers.Features;

public static class UpdateCustomer
{
    public sealed record Command(
        Guid CustomerId,
        string Email,
        string VirtualWalletAccountId) : ICommand;

    internal sealed class CommandHandler : ICommandHandler<Command>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            var virtualWalletAccountResult = VirtualWalletAccount.Create(request.VirtualWalletAccountId);

            if (!await _customerRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
            {
                return Result.Failure<Guid>(DomainErrors.Customer.EmailAlreadyInUse);
            }

            var member = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (member is null)
            {
                return Result.Failure(
                    DomainErrors.Customer.NotFound(request.CustomerId));
            }

            member.Email = emailResult.Value;
            member.VirtualWalletAccount = virtualWalletAccountResult.Value;

            _customerRepository.Update(member);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    internal class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.VirtualWalletAccountId).NotEmpty().Must(input => Guid.TryParse(input, out _)).WithErrorCode("Not a guid");
        }
    }
}
public static class CreateCustomer
{
    public sealed record Command(
        string Email,
        string FirstName,
        string LastName,
        string VirtualWalletAccount) : ICommand<Guid>;

    internal sealed class CommandHandler : ICommandHandler<Command, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            var firstNameResult = FirstName.Create(request.FirstName);
            var lastNameResult = LastName.Create(request.LastName);
            var virtualWalletAccountResult = VirtualWalletAccount.Create(request.VirtualWalletAccount);

            if (!await _customerRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
            {
                return Result.Failure<Guid>(DomainErrors.Customer.EmailAlreadyInUse);
            }

            var member = Customer.Create(
                Guid.NewGuid(),
                emailResult.Value,
                firstNameResult.Value,
                lastNameResult.Value,
                virtualWalletAccountResult.Value);

            _customerRepository.Add(member);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return member.Id;
        }
    }

    internal class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(LastName.MaxLength);

            RuleFor(x => x.VirtualWalletAccount).NotEmpty().Must(input => Guid.TryParse(input, out _)).WithErrorCode("Not a guid");
        }
    }
}
public static class GetCustomerById
{
    public sealed record Query(Guid Id) : IQuery<CustomerResponse>;

    internal sealed class QueryHandler
        : IQueryHandler<Query, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public QueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<CustomerResponse>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (customer is null)
            {
                return Result.Failure<CustomerResponse>(
                    DomainErrors.Customer.NotFound(request.Id));
            }

            return new CustomerResponse(customer.Id,
                customer.Email.Value,
                customer.FirstName.Value,
                customer.LastName.Value,
                customer.VirtualWalletAccount.Value);
        }
    }
}

public sealed record UpdateCustomerRequest(string Email, string VirtualWalletAccountId);
public sealed record CreateCustomerRequest(string Email, string FirstName, string LastName, string VirtualWalletAccountId);
public sealed record CustomerResponse(Guid Id, string Email, string FirstName, string LastName, Guid virtualWalletAccount);

public class CustomerEndpoints : MinimalApiEndpointBase, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customer",
            async (CreateCustomerRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateCustomer.Command(
                request.Email,
                request.FirstName,
                request.LastName,
                request.VirtualWalletAccountId);

            var result = await sender.Send(command, cancellationToken);

            return result.IsFailure
                ? HandleFailure(result)
                : Results.CreatedAtRoute(
                    nameof(GetCustomerById),
                    new { id = result.Value },
                    result.Value);
        });

        app.MapPut("api/customer",
            async (Guid id, UpdateCustomerRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateCustomer.Command(
                id,
                request.Email,
                request.VirtualWalletAccountId);

            var result = await sender.Send(command, cancellationToken);

            return result.IsFailure
                ? HandleFailure(result)
                : Results.NoContent();
        });

        app.MapGet("api/customer/{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetCustomerById.Query(id);

            var response = await sender.Send(query, cancellationToken);

            return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Error);

        }).WithName(nameof(GetCustomerById));
    }
}
