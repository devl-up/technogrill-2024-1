using FluentValidation;
using MediatR;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Features.Orders.Commands;

public static class AddOrder
{
    public sealed record Command(Guid Id) : IRequest;

    public sealed class Handler(IValidator<Command> validator, IOrderRepository orderRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var order = new Order(request.Id);
            await orderRepository.Save(order);
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