using FluentValidation;
using MediatR;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Features.Orders.Commands;

public static class DeleteOrderItem
{
    public sealed record Command(Guid Id, Guid ItemId) : IRequest;

    public sealed class Handler(IValidator<Command> validator, IOrderRepository orderRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var (id, itemId) = request;

            var order = await orderRepository.GetById(id);
            order.DeleteItem(itemId);
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
        }
    }
}