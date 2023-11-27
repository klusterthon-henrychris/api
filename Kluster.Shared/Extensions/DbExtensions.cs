using Kluster.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;

namespace Kluster.Shared.Extensions;

public static class DbExtensions
{
    public static void AddDatabase<T>(IServiceCollection services) where T : DbContext
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string connectionString;

        if (env == Environments.Development)
        {
            var dbSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<DatabaseSettings>>()?.Value;
            connectionString = dbSettings!.ConnectionString!;
        }
        else
        {
            // Use connection string provided at runtime by Fly.
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Parse connection URL to connection string for Npgsql
            connUrl = connUrl.Replace("postgres://", string.Empty);
            var pgUserPass = connUrl.Split("@")[0];
            var pgHostPortDb = connUrl.Split("@")[1];
            var pgHostPort = pgHostPortDb.Split("/")[0];
            var pgDb = pgHostPortDb.Split("/")[1];
            var pgUser = pgUserPass.Split(":")[0];
            var pgPass = pgUserPass.Split(":")[1];
            var pgHost = pgHostPort.Split(":")[0];
            var pgPort = pgHostPort.Split(":")[1];
            var updatedHost = pgHost.Replace("flycast", "internal");

            connectionString =
                $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
            Console.WriteLine($"ConnectionString: {connectionString}");
        }

        services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName, typeof(T).Name));
        });


        var dbContext = services.BuildServiceProvider().GetRequiredService<T>();
        dbContext.Database.Migrate();
    }
}