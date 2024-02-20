using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using TechnoGrill.Infrastructure.Data;

namespace TechnoGrill.IntegrationTests.Common;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private Respawner? _respawner;

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        await context.Database.OpenConnectionAsync();
        _respawner = await Respawner.CreateAsync(context.Database.GetDbConnection(), new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING"))
            .Options;

        return new AppDbContext(options);
    }

    public async Task ResetDb()
    {
        if (_respawner == null) return;
        await using var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING"));
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }
}

[CollectionDefinition("IntegrationTests")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;