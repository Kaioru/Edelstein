using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Common.Services.Social;

public class SocialDbContextFactory : IDesignTimeDbContextFactory<SocialDbContext>
{
    public SocialDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("devsettings.json", true)
            .AddJsonFile("devsettings.Development.json", true)
            .Build();
        var connection = configuration.GetConnectionString(SocialDbContext.ConnectionStringKey);
        var builder = new DbContextOptionsBuilder<SocialDbContext>().UseNpgsql(connection);

        return new SocialDbContext(builder.Options);
    }
}
