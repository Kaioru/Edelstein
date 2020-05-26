using Edelstein.Core.Provider;
using Edelstein.Core.Provider.NX;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Parsing
{
    public class NXParsingProvider : IProvider
    {
        private readonly string _path;

        public NXParsingProvider(string connectionString)
            => _path = connectionString;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataDirectoryCollection, NXDataDirectoryCollection>(f =>
                new NXDataDirectoryCollection(_path)
            );
        }
    }
}