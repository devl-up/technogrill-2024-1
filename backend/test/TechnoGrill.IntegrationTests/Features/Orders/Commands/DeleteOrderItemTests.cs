using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Commands;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Orders.Commands;

[Collection("IntegrationTests")]
public sealed class DeleteOrderItemTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task DeleteOrderItem_Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var orderId = Guid.NewGuid();
        var order = new Order(orderId);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Price = 10
        };

        var orderItemId = Guid.NewGuid();
        order.AddItem(orderItemId, product.Id, 10);

        await using (var context = DatabaseFixture.CreateContext())
        {
            await context.Set<OrderMemento>().AddAsync(order.ToMemento());
            await context.Set<Product>().AddAsync(product);
            await context.SaveChangesAsync();
        }

        var command = new DeleteOrderItem.Command(orderId, orderItemId);

        // Act
        var response = await client.PostAsJsonAsync("api/order/delete-item", command);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using (var context = DatabaseFixture.CreateContext())
        {
            var orderItemCount = await context.Set<OrderItemMemento>()
                .CountAsync(o => o.Id == orderItemId);

            Assert.Equal(0, orderItemCount);
        }
    }
}