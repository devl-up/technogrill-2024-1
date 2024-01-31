using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Enums;

namespace TechnoGrill.Tests.Features.Orders.Entities;

public sealed class OrderTests
{
    [Fact]
    public void Constructor_Should_CreateCorrectOrder_When_Successful()
    {
        // Act
        var order = new Order(Guid.NewGuid());

        // Assert
        var memento = order.ToMemento();

        Assert.Equal(OrderStatus.Pending, memento.Status);
        Assert.Empty(memento.Items);
    }

    [Fact]
    public void AddItem_Should_AddOrderItem_When_Successful()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act
        order.AddItem(Guid.NewGuid(), Guid.NewGuid(), 10);

        // Assert
        var memento = order.ToMemento();

        Assert.Single(memento.Items);
    }

    [Fact]
    public void AddItem_Should_ThrowException_When_StatusIsNotPending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Approve();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.AddItem(Guid.NewGuid(), Guid.NewGuid(), 10));
    }

    [Fact]
    public void AddItem_Should_ThrowException_When_AmountIsInvalid()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.AddItem(Guid.NewGuid(), Guid.NewGuid(), 0));
    }

    [Fact]
    public void AddItem_Should_ThrowException_When_ProductIsAlreadyAdded()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), productId, 10);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.AddItem(Guid.NewGuid(), productId, 10));
    }

    [Fact]
    public void ChangeAmount_Should_ChangeItemAmount_When_Successful()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(itemId, Guid.NewGuid(), 10);
        
        // Act
        order.ChangeAmount(itemId, 20);
        
        // Assert
        var item = order.ToMemento().Items.First(i => i.Id == itemId);
        Assert.Equal(20, item.Amount);
    }
    
    [Fact]
    public void ChangeAmount_Should_ThrowException_When_StatusIsNotPending()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(itemId, Guid.NewGuid(), 10);
        order.Approve();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.ChangeAmount(itemId, 20));
    }
    
    [Fact]
    public void ChangeAmount_Should_ThrowException_When_AmountIsInvalid()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(itemId, Guid.NewGuid(), 10);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.ChangeAmount(itemId, 0));
    }
    
    [Fact]
    public void ChangeAmount_Should_ThrowException_When_ItemDoesNotExist()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), Guid.NewGuid(), 10);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.ChangeAmount(Guid.NewGuid(), 0));
    }

    [Fact]
    public void DeleteItem_Should_DeleteOrderItem_When_Successful()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(itemId, Guid.NewGuid(), 10);
        
        // Act
        order.DeleteItem(itemId);
        
        // Assert
        var memento = order.ToMemento();
        Assert.Empty(memento.Items);
    }
    
    [Fact]
    public void DeleteItem_Should_ThrowException_When_StatusIsNotPending()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid());
        order.AddItem(itemId, Guid.NewGuid(), 10);
        order.Approve();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.DeleteItem(itemId));
    }
    
    [Fact]
    public void DeleteItem_Should_ThrowException_When_ItemDoesNotExist()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), Guid.NewGuid(), 10);
        order.Approve();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.DeleteItem(Guid.NewGuid()));
    }

    [Fact]
    public void Approve_Should_SetStatusToApproved_When_Successful()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        
        // Act
        order.Approve();
        
        // Assert
        var memento = order.ToMemento();
        Assert.Equal(OrderStatus.Approved, memento.Status);
    }
    
    [Fact]
    public void Approve_Should_ThrowException_When_StatusIsNotPending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Approve();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.Approve());
    }
    
    [Fact]
    public void Decline_Should_SetStatusToDeclined_When_Successful()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        
        // Act
        order.Decline();
        
        // Assert
        var memento = order.ToMemento();
        Assert.Equal(OrderStatus.Declined, memento.Status);
    }
    
    [Fact]
    public void Decline_Should_ThrowException_When_StatusIsNotPending()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Decline();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.Approve());
    }
}