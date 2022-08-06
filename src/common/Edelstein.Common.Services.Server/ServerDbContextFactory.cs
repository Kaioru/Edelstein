using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Services.Server;

public class ServerDbContextFactory : IDesignTimeDbContextFactory<ServerDbContext>
{
    public ServerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(ServerDbContext.ConnectionStringKey);
        var builder = new DbContextOptionsBuilder<ServerDbContext>().UseNpgsql(connection);

        return new ServerDbContext(builder.Options);
    }
}
