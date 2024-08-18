using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Database.Pgsql;

public class PgsqlDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public static readonly GameDbContextProvider Pgsql = new(nameof(Pgsql), typeof(PgsqlDbMarker).Assembly);
    
    public GameDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("devsettings.json", true)
            .AddJsonFile("devsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(Pgsql.Key);
        var builder = new DbContextOptionsBuilder<GameDbContext>().UseNpgsql(
            connection,
            options => options.MigrationsAssembly(Pgsql.Assembly.FullName)
        );

        return new GameDbContext(builder.Options);
    }
}
