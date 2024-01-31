using FluentValidation;
using MediatR;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Features.Orders.Commands;

public static class DeleteOrder
{
    public sealed record Command(Guid Id) : IRequest;

    public sealed class Handler(IValidator<Command> validator, IOrderRepository orderRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            await orderRepository.Delete(request.Id);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id can't be empty");
        }
    }
}