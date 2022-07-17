﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Gameplay.Database;

public class GameplayDbContextFactory : IDesignTimeDbContextFactory<GameplayDbContext>
{
    public GameplayDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(GameplayDbContext.ConnectionStringKey);
        var builder = new DbContextOptionsBuilder<GameplayDbContext>().UseNpgsql(connection);

        return new GameplayDbContext(builder.Options);
    }
}
