using TechnoGrill.Features.Orders.Entities;

namespace TechnoGrill.Features.Orders.Repositories;

public interface IOrderRepository
{
    Task<Order> GetById(Guid id);
    Task Save(Order order);
    Task Delete(Guid id);
}