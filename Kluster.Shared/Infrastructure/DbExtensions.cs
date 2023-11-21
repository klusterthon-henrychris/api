using Kluster.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kluster.Shared.Infrastructure;

public static class DbExtensions
{
    public static void AddDatabase<T>(IServiceCollection services) where T : DbContext
    {
        var dbSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<DatabaseSettings>>()?.Value;
        services.AddDbContext<T>(options => options.UseSqlServer(dbSettings!.ConnectionString));

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.Migrate();
    }
}