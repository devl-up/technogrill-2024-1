using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.Features.Products.Commands;

public static class DeleteProduct
{
    public sealed record Command(Guid Id) : IRequest;

    public sealed class Handler(IValidator<Command> validator, AppDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            await dbContext
                .Set<Product>()
                .Where(p => p.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
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