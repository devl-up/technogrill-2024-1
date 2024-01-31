using FluentValidation;
using MediatR;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Features.Orders.Commands;

public static class AddOrderItem
{
    public sealed record Command(Guid Id, Guid ProductId, int Amount) : IRequest;
    
    public sealed class Handler(IValidator<Command> validator, IOrderRepository orderRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var (id, productId, amount) = request;

            var order = await orderRepository.GetById(id);
            order.AddItem(Guid.NewGuid(), productId, amount);
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
            
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .WithMessage("Product can't be empty");
            
            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage("Amount can't be empty");
        }
    }
}