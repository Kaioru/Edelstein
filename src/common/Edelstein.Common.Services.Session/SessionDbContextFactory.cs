using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Services.Session;

public class SessionDbContextFactory : IDesignTimeDbContextFactory<SessionDbContext>
{
    public SessionDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(SessionDbContext.ConnectionStringKey);
        var builder = new DbContextOptionsBuilder<SessionDbContext>().UseNpgsql(connection);

        return new SessionDbContext(builder.Options);
    }
}
