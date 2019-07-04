using Edelstein.Provider.Parsing;
using Edelstein.Provider.Parsing.NX;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Parser
{
    public class NXDataParserProvider : AbstractDataParserProvider
    {
        public NXDataParserProvider(string path) : base(path)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataDirectoryCollection>(
                new NXDataDirectoryCollection(Path)
            );
        }
    }
}