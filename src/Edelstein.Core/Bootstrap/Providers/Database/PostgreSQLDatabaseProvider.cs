using Edelstein.Database.Store;
using Edelstein.Database.Store.Postgres;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class PostgreSQLDatabaseProvider : AbstractDatabaseProvider
    {
        public PostgreSQLDatabaseProvider(string connectionString) : base(connectionString)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataStore>(f =>
                new MartenDataStore(DocumentStore.For(ConnectionString))
            );
        }
    }
}