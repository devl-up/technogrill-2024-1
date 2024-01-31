using FluentValidation;
using MediatR;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Features.Orders.Commands;

public static class ChangeOrderItemAmount
{
    public sealed record Command(Guid Id, Guid ItemId, int Amount) : IRequest;

    public sealed class Handler(IValidator<Command> validator, IOrderRepository orderRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var (id, itemId, amount) = request;

            var order = await orderRepository.GetById(id);
            order.ChangeAmount(itemId, amount);
            await orderRepository.Save(order);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id can't be empty");
            
            RuleFor(c => c.ItemId)
                .NotEmpty()
                .WithMessage("Item can't be empty");
            
            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage("Amount can't be empty");
        }
    }
}