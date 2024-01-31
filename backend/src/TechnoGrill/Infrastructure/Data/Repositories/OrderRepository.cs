using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.Features.Orders.Repositories;

namespace TechnoGrill.Infrastructure.Data.Repositories;

public sealed class OrderRepository(AppDbContext context) : IOrderRepository
{
    public Task<Order> GetById(Guid id)
    {
        return context
            .Set<OrderMemento>()
            .AsNoTracking()
            .Include(m => m.Items)
            .Where(m => m.Id == id)
            .Select(m => new Order(m))
            .FirstAsync();
    }

    public async Task Save(Order order)
    {
        var newOrder = order.ToMemento();

        var existingOrder = await context
            .Set<OrderMemento>()
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.Id == newOrder.Id);

        if (existingOrder == null)
        {
            await context.Set<OrderMemento>().AddAsync(newOrder);
        }
        else
        {
            context.Entry(existingOrder).CurrentValues.SetValues(newOrder);

            foreach (var newOrderItem in newOrder.Items)
            {
                var existingOrderItem = existingOrder.Items.FirstOrDefault(i => i.Id == newOrderItem.Id);

                if (existingOrderItem == null)
                {
                    await context.Set<OrderItemMemento>().AddAsync(newOrderItem);
                }
                else
                {
                    context.Entry(existingOrderItem).CurrentValues.SetValues(newOrderItem);
                }
            }

            foreach (var existingOrderItem in existingOrder.Items.Where(existingOrderItem => newOrder.Items.All(i => i.Id != existingOrderItem.Id)))
            {
                context.Set<OrderItemMemento>().Remove(existingOrderItem);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task Delete(Guid id)
    {
        return context.Set<OrderMemento>()
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();
    }
}