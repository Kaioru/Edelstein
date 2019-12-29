using Edelstein.Database.Json;
using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IDataStore = Edelstein.Database.IDataStore;

namespace Edelstein.Core.Bootstrap.Providers.Database
{
    public class JsonDatabaseProvider : IProvider
    {
        private readonly string _path;

        public JsonDatabaseProvider(string path)
            => _path = path;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<DataStore>(f => new DataStore(_path));
            collection.AddSingleton<IDataStore, JsonDataStore>();
        }
    }
}