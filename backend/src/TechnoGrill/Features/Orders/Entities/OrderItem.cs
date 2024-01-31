namespace TechnoGrill.Features.Orders.Entities;

public sealed record OrderItem(Guid Id, Guid ProductId, int Amount);