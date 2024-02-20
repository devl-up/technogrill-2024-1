using System.Net;
using System.Text;
using System.Text.Json;
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
public sealed class DeleteOrderTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task DeleteOrder_Should_ReturnNoContent_When_Successful()
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

        var command = new DeleteOrder.Command(orderId);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Delete, "api/order");
        request.Content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using (var context = DatabaseFixture.CreateContext())
        {
            var orderCount = await context.Set<OrderMemento>()
                .CountAsync();

            Assert.Equal(0, orderCount);
        }
    }
}