using MediatR;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Enums;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.Features.Orders.Queries;

public static class GetOrders
{
    public sealed record Query : IRequest<List<Dto>>;

    public sealed record Dto(Guid Id, OrderStatus Status);

    public sealed class Handler(AppDbContext dbContext) : IRequestHandler<Query, List<Dto>>
    {
        public Task<List<Dto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return dbContext.Set<OrderMemento>()
                .AsNoTracking()
                .OrderBy(o => o.Id)
                .Select(o => new Dto(o.Id, o.Status))
                .ToListAsync(cancellationToken);
        }
    }
}