using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Services.Auth;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(AuthDbContext.ConnectionStringKey);
        var builder = new DbContextOptionsBuilder<AuthDbContext>().UseNpgsql(connection);

        return new AuthDbContext(builder.Options);
    }
}
