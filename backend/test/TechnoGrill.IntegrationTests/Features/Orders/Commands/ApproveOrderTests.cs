using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Commands;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Enums;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Orders.Commands;

[Collection("IntegrationTests")]
public sealed class ApproveOrderTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task ApproveOrder_Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var orderId = Guid.NewGuid();
        var order = new Order(orderId);

        await using (var context = DatabaseFixture.CreateContext())
        {
            await context.Set<OrderMemento>().AddAsync(order.ToMemento());
            await context.SaveChangesAsync();
        }

        var command = new ApproveOrder.Command(orderId);

        // Act
        var response = await client.PostAsJsonAsync("api/order/approve", command);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using (var context = DatabaseFixture.CreateContext())
        {
            var orderCount = await context.Set<OrderMemento>()
                .CountAsync(o => o.Id == command.Id && o.Status == OrderStatus.Approved);

            Assert.Equal(1, orderCount);
        }
    }
}