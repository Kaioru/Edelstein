using System;
using Edelstein.Core.Bootstrap.Providers.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredCachingProvider : IProvider
    {
        private readonly string _section;

        public ConfiguredCachingProvider(string section)
            => _section = section;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            var section = context.Configuration.GetSection(_section);
            var type = Enum.Parse<CachingProviderTypes>(section["Type"]);

            switch (type)
            {
                case CachingProviderTypes.InMemory:
                    new InMemoryCachingProvider().Provide(context, collection);
                    break;
                case CachingProviderTypes.Redis:
                    new RedisCachingProvider(section["ConnectionString"]).Provide(context, collection);
                    break;
            }
        }
    }
}