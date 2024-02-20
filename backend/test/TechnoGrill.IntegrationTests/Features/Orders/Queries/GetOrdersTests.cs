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
public sealed class GetOrdersTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task GetOrders_Should_ReturnOk_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var orders = new List<Order>
        {
            new(Guid.NewGuid()),
            new(Guid.NewGuid())
        };

        await using (var context = DatabaseFixture.CreateContext())
        {
            await context.Set<OrderMemento>().AddRangeAsync(orders.Select(o => o.ToMemento()));
            await context.SaveChangesAsync();
        }

        // Act
        var response = await client.GetAsync("api/order");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<List<GetOrders.Dto>>(JsonSerializerOptions.Default);
        Assert.NotNull(content);
        Assert.Equal(2, content.Count);
    }
}