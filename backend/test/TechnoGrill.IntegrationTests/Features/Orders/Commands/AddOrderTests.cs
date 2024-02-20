using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Orders.Commands;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Orders.Commands;

[Collection("IntegrationTests")]
public sealed class AddOrderTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task AddOrder_Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var command = new AddOrder.Command(Guid.NewGuid());

        // Act
        var response = await client.PostAsJsonAsync("api/order", command);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = DatabaseFixture.CreateContext();

        var orderCount = await context.Set<OrderMemento>()
            .CountAsync(o => o.Id == command.Id);

        Assert.Equal(1, orderCount);
    }
}