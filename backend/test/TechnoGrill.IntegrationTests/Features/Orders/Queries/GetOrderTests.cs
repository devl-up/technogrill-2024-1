using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TechnoGrill.Features.Orders.Entities;
using TechnoGrill.Features.Orders.Mementos;
using TechnoGrill.Features.Orders.Queries;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Orders.Queries;

[Collection("IntegrationTests")]
public sealed class GetOrderTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task GetOrder_Should_ReturnOk_When_Successful()
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

        // Act
        var response = await client.GetAsync($"api/order/{orderId}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<GetOrder.Dto?>(JsonSerializerOptions.Default);
        Assert.NotNull(content);
    }
}