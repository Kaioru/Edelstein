using Edelstein.Database.Store;
using Edelstein.Database.Store.LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class LiteDBDatabaseProvider : AbstractDatabaseProvider
    {
        public LiteDBDatabaseProvider(string connectionString) : base(connectionString)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataStore>(f =>
                new LiteDBDataStore(ConnectionString)
            );
        }
    }
}