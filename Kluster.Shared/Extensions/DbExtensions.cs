using Kluster.Shared.Configuration;
using Kluster.Shared.Domain;
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
            connectionString = SharedLogic.GetProdPostGresConnectionString();
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