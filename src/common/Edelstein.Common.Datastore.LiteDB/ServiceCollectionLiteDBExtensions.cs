using Edelstein.Protocol.Datastore;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Common.Datastore.LiteDB
{
    public static class ServiceCollectionLiteDBExtensions
    {
        public static void AddLiteDBDataStore(this IServiceCollection c, string connectionString)
        {
            c.AddSingleton<ILiteRepository>(new LiteRepository(connectionString));
            c.AddSingleton<IDataStore, LiteDataStore>();
        }
    }
}
