using Edelstein.Database;
using Edelstein.Database.LiteDB;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class LiteDBDatabaseProvider : IProvider
    {
        private readonly string _connectionString;

        public LiteDBDatabaseProvider(string connectionString)
            => _connectionString = connectionString;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<LiteRepository>(f => new LiteRepository(_connectionString));
            collection.AddSingleton<IDataStore, LiteDBDataStore>();
        }
    }
}