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
public sealed class ChangeProductTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task ChangeProduct_Should_ReturnNoContent_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Price = 10
        };

        await using (var context = DatabaseFixture.CreateContext())
        {
            await context.Set<Product>().AddAsync(product);
            await context.SaveChangesAsync();
        }

        var command = new ChangeProduct.Command(product.Id, "newName", "newDescription", 20);

        // Act
        var response = await client.PutAsJsonAsync("api/product", command);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using (var context = DatabaseFixture.CreateContext())
        {
            var expectedProduct = new { command.Name, command.Description, command.Price };

            var newProduct = await context.Set<Product>()
                .Where(p => p.Id == command.Id)
                .Select(p => new { p.Name, p.Description, p.Price })
                .FirstAsync();

            Assert.Equivalent(expectedProduct, newProduct);
        }
    }
}