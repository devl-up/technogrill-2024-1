using MediatR;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Enums;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.Features.Orders.Queries;

public static class GetOrder
{
    public sealed record Query(Guid Id) : IRequest<Dto?>;

    public sealed record ItemDto(Guid Id, int Amount, Guid ProductId, string ProductName, int ProductPrice);

    public sealed record Dto(Guid Id, OrderStatus Status, IEnumerable<ItemDto> Items);

    public sealed class Handler(AppDbContext dbContext) : IRequestHandler<Query, Dto?>
    {
        public Task<Dto?> Handle(Query request, CancellationToken cancellationToken)
        {
            return dbContext.Set<OrderMemento>()
                .AsNoTracking()
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == request.Id)
                .Select(o => new Dto(
                    o.Id,
                    o.Status,
                    o.Items.OrderBy(i => i.Product!.Name).Select(i => new ItemDto(i.Id, i.Amount, i.ProductId, i.Product!.Name, i.Product.Price)))
                )
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}