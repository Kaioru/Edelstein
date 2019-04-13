using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Database
{
    public class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("database.example.json", true);
            builder.AddJsonFile("database.json", true);
            builder.AddCommandLine(args);

            var config = builder.Build();

            options.UseMySql(config["DatabaseConnectionString"]);
            return new DataContext(options.Options);
        }
    }
}