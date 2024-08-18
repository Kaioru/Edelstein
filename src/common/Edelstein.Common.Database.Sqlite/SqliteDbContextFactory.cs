using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Database.Sqlite;

public class SqliteDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public static readonly GameDbContextProvider Sqlite = new(nameof(Sqlite), typeof(SqliteDbMarker).Assembly);
    
    public GameDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("devsettings.json", true)
            .AddJsonFile("devsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(Sqlite.Key);
        var builder = new DbContextOptionsBuilder<GameDbContext>().UseSqlite(
            connection,
            options => options.MigrationsAssembly(Sqlite.Assembly.FullName)
        );

        return new GameDbContext(builder.Options);
    }
}
