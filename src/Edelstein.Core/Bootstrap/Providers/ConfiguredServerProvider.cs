using System;
using Edelstein.Core.Network;
using Edelstein.Core.Network.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredServerProvider<TAdapterFactory> : IProvider
        where TAdapterFactory : class, ISocketAdapterFactory
    {
        private readonly string _section;

        public ConfiguredServerProvider(string section)
            => _section = section;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            var section = context.Configuration.GetSection(_section);

            collection.AddSingleton<ISocketAdapterFactory, TAdapterFactory>();
            collection.AddSingleton<Server>(f => new Server(
                f.GetService<ISocketAdapterFactory>(),
                Convert.ToInt16(section["Version"]),
                section["Patch"],
                Convert.ToByte(section["Locale"])
            ));
        }
    }
}