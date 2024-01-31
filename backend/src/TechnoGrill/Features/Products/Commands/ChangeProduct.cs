using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.Features.Products.Commands;

public static class ChangeProduct
{
    public sealed record Command(Guid Id, string Name, string Description, int Price) : IRequest;

    public sealed class Handler(IValidator<Command> validator, AppDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var (id, name, description, price) = request;

            await dbContext.Set<Product>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(calls => calls
                    .SetProperty(p => p.Name, name)
                    .SetProperty(p => p.Description, description)
                    .SetProperty(p => p.Price, price), cancellationToken);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id can't be empty");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name can't be empty")
                .MaximumLength(50).WithMessage("Name can't exceed 50 characters");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description can't be empty")
                .MaximumLength(200).WithMessage("Description can't exceed 200 characters");

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("Price can't be empty");
        }
    }
}