using TechnoGrill.Features.Orders.Enums;

namespace TechnoGrill.Features.Orders.Mementos;

public sealed class OrderMemento
{
    public List<OrderItemMemento> Items = [];
    public required Guid Id { get; init; }
    public required OrderStatus Status { get; init; }
}