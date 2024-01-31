using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Products.Commands;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Products.Commands;

[Collection("IntegrationTests")]
public sealed class AddProductTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task AddProduct_Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var command = new AddProduct.Command(Guid.NewGuid(), "name", "description", 10);

        // Act
        var response = await client.PostAsJsonAsync("api/product", command);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = databaseFixture.CreateContext();

        var productCount = await context.Set<Product>()
            .CountAsync(p => p.Id == command.Id);

        Assert.Equal(1, productCount);
    }
}