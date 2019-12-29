using Edelstein.Database.Postgres;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IDataStore = Edelstein.Database.IDataStore;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class MartenDatabaseProvider : IProvider
    {
        private readonly string _connectionString;

        public MartenDatabaseProvider(string connectionString)
            => _connectionString = connectionString;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDocumentStore>(f => DocumentStore.For(_connectionString));
            collection.AddSingleton<IDataStore, MartenDataStore>();
        }
    }
}