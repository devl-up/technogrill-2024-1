using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TechnoGrill.Features.Products.Commands;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Products.Commands;

[Collection("IntegrationTests")]
public sealed class DeleteProductTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task DeleteProduct_Should_ReturnNoContent_When_Successful()
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

        await using (var context = databaseFixture.CreateContext())
        {
            await context.Set<Product>().AddAsync(product);
            await context.SaveChangesAsync();
        }

        var command = new DeleteProduct.Command(product.Id);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Delete, "api/product");
        request.Content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using (var context = databaseFixture.CreateContext())
        {
            var productCount = await context.Set<Product>()
                .CountAsync();

            Assert.Equal(0, productCount);
        }
    }
}