using Edelstein.Provider.Parsing;
using Edelstein.Provider.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class TemplateProvider : AbstractProvider
    {
        private readonly TemplateCollectionType _type;

        public TemplateProvider(TemplateCollectionType type)
        {
            _type = type;
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<ITemplateManager>(f =>
                {
                    var manager = new TemplateManager(
                        f.GetService<IDataDirectoryCollection>(),
                        _type
                    );

                    manager.Load().Wait();
                    return manager;
                });
        }
    }
}