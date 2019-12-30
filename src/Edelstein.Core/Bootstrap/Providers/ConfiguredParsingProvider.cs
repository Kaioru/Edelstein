using System;
using Edelstein.Core.Bootstrap.Providers.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredParsingProvider : IProvider
    {
        private readonly string _section;

        public ConfiguredParsingProvider(string section)
            => _section = section;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            var section = context.Configuration.GetSection(_section);
            var type = Enum.Parse<ParsingProviderType>(section["Type"]);

            switch (type)
            {
                case ParsingProviderType.NX:
                    new NXParsingProvider(section["Path"]).Provide(context, collection);
                    break;
            }
        }
    }
}