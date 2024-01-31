using MediatR;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.Features.Products.Queries;

public static class GetProducts
{
    public sealed record Query : IRequest<List<Dto>>;

    public sealed record Dto(Guid Id, string Name, string Description, int Price);

    public sealed class Handler(AppDbContext dbContext) : IRequestHandler<Query, List<Dto>>
    {
        public Task<List<Dto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return dbContext.Set<Product>()
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new Dto(p.Id, p.Name, p.Description, p.Price))
                .ToListAsync(cancellationToken);
        }
    }
}