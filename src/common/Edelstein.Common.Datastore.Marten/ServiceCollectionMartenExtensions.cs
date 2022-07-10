using Edelstein.Protocol.Datastore;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Common.Datastore.Marten
{
    public static class ServiceCollectionMartenExtensions
    {
        public static void AddPostgresDataStore(this IServiceCollection c, string connectionString)
        {
            c.AddSingleton<IDocumentStore>(DocumentStore.For(connectionString));
            c.AddSingleton<IDataStore, PostgresDataStore>();
        }
    }
}

