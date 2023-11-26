﻿using Kluster.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;

namespace Kluster.Shared.Extensions;

public static class DbExtensions
{
    public static void AddDatabase<T>(IServiceCollection services) where T : DbContext
    {
        var dbSettings = services.BuildServiceProvider().GetService<IOptionsSnapshot<DatabaseSettings>>()?.Value;
        services.AddDbContext<T>(options =>
        {
            options.UseNpgsql(dbSettings!.ConnectionString, o => o.MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName, typeof(T).Name));
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.Migrate();
    }
}