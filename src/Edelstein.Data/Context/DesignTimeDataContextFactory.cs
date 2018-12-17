using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Edelstein.Data.Context
{
    public class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Database.example.json")
                .AddJsonFile("Database.json", true);
            var config = configBuilder.Build();

            optionsBuilder.UseMySql(config["ConnectionString"]);
            return new DataContext(optionsBuilder.Options);
        }
    }
}