using TechnoGrill.Features.Products.Entities;

namespace TechnoGrill.Features.Orders.Mementos;

public sealed class OrderItemMemento
{
    public required Guid Id { get; init; }
    public required Guid ProductId { get; init; }
    public required Guid OrderId { get; init; }
    public required int Amount { get; init; }

    public Product? Product { get; } = null;
    public OrderMemento? Order { get; } = null;
}