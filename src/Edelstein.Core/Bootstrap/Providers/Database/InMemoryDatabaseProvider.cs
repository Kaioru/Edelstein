using Edelstein.Database.Store;
using Edelstein.Database.Store.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class InMemoryDatabaseProvider : AbstractDatabaseProvider
    {
        public InMemoryDatabaseProvider(string connectionString) : base(connectionString)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataStore, InMemoryDataStore>();
        }
    }
}