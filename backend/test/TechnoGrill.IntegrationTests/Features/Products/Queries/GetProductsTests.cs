using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TechnoGrill.Features.Products.Entities;
using TechnoGrill.Features.Products.Queries;
using TechnoGrill.IntegrationTests.Common;
using TechnoGrill.IntegrationTests.Extensions;

namespace TechnoGrill.IntegrationTests.Features.Products.Queries;

[Collection("IntegrationTests")]
public class GetProductsTests(DatabaseFixture databaseFixture)
{
    [Fact]
    public async Task GetProducts_Should_ReturnOk_When_Successful()
    {
        // Arrange
        await databaseFixture.ResetDb();
        await using var factory = new WebApplicationFactory<Program>().WithDatabase();
        using var client = factory.CreateClient();

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Description = "description",
                Price = 10
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Description = "description",
                Price = 10
            }
        };

        await using (var context = DatabaseFixture.CreateContext())
        {
            await context.Set<Product>().AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Act
        var response = await client.GetAsync("api/product");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<List<GetProducts.Dto>>(JsonSerializerOptions.Default);
        Assert.NotNull(content);
        Assert.Equal(2, content.Count);
    }
}