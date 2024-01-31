using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.IntegrationTests.Extensions;

public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<T> WithDatabase<T>(this WebApplicationFactory<T> factory) where T : class
    {
        return factory.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.Single(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            services.Remove(dbContextOptionsDescriptor);

            var dbContextDescriptor = services.Single(
                d => d.ServiceType ==
                     typeof(AppDbContext));

            services.Remove(dbContextDescriptor);

            services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")); });
        }));
    }
}