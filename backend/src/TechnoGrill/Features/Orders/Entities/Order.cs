using TechnoGrill.Features.Orders.Enums;
using TechnoGrill.Features.Orders.Mementos;

namespace TechnoGrill.Features.Orders.Entities;

public sealed class Order
{
    private readonly Guid _id;
    private readonly List<OrderItem> _items;
    private OrderStatus _status;

    public Order(Guid id)
    {
        _id = id;
        _items = [];
        _status = OrderStatus.Pending;
    }

    public Order(OrderMemento memento)
    {
        _id = memento.Id;
        _items = memento.Items.Select(i => new OrderItem(i.Id, i.ProductId, i.Amount)).ToList();
        _status = memento.Status;
    }

    public OrderMemento ToMemento()
    {
        return new OrderMemento
        {
            Id = _id,
            Items = _items.Select(i => new OrderItemMemento
            {
                Id = i.Id,
                OrderId = _id,
                ProductId = i.ProductId,
                Amount = i.Amount
            }).ToList(),
            Status = _status
        };
    }

    public void AddItem(Guid id, Guid productId, int amount)
    {
        CheckStatus();
        CheckAmount(amount);

        if (_items.Any(i => i.ProductId == productId))
        {
            throw new InvalidOperationException("Product is already added to order");
        }

        var item = new OrderItem(id, productId, amount);
        _items.Add(item);
    }

    public void ChangeAmount(Guid id, int amount)
    {
        CheckStatus();
        CheckAmount(amount);

        var index = _items.FindIndex(i => i.Id == id);

        if (index == -1)
        {
            throw new InvalidOperationException("Item not found");
        }

        var item = _items[index];
        _items[index] = item with { Amount = amount };
    }

    public void DeleteItem(Guid id)
    {
        CheckStatus();

        var index = _items.FindIndex(i => i.Id == id);

        if (index == -1)
        {
            throw new InvalidOperationException("Item not found");
        }

        _items.RemoveAt(index);
    }

    public void Approve()
    {
        CheckStatus();
        _status = OrderStatus.Approved;
    }

    public void Decline()
    {
        CheckStatus();
        _status = OrderStatus.Declined;
    }

    private static void CheckAmount(int amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Amount can't be empty");
        }
    }

    private void CheckStatus()
    {
        if (_status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Can only work on pending orders");
        }
    }
}