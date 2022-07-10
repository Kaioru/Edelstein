using Edelstein.Common.Network.DotNetty.Transport;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Network.DotNetty
{
    public static class ServiceCollectionDotNettyExtensions
    {
        public static void AddNettyAcceptor(this IServiceCollection c, short version, string patch, byte locale)
        {
            c.AddSingleton<ITransportAcceptor>(p => new NettyTransportAcceptor(
                p.GetService<ISessionInitializer>(),
                version,
                patch,
                locale,
                p.GetService<ILogger<ITransportAcceptor>>()
            ));
        }

        public static void AddNettyConnector(this IServiceCollection c, short version, string patch, byte locale)
        {
            c.AddSingleton<ITransportConnector>(p => new NettyTransportConnector(
                p.GetService<ISessionInitializer>(),
                version,
                patch,
                locale,
                p.GetService<ILogger<ITransportConnector>>()
            ));
        }
    }
}
