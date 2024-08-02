using System.Threading.Tasks;
using DotNetty.Common.Concurrency;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Crypto;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportConnector<TSocketUser>(
    TransportVersion version, 
    ISocketUserAdapter<TSocketUser> adapter,
    ISocketUserInitializer<TSocketUser> initializer
) : ITransportAcceptor
    where TSocketUser : class, ISocketUser
{
    public async Task<ITransportContext> Accept(string host, int port)
    {
        var cipherAES = new CipherAES();
        var cipherIG = new CipherIG();

        var group = new DefaultChannelGroup(new ImmediateEventExecutor());
        var group0 = new MultithreadEventLoopGroup();
        var channel = await new Bootstrap()
            .Group(group0)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new NettyPacketDecoder(version, cipherAES, cipherIG),
                    new NettyTransportConnectorHandler<TSocketUser>(version, adapter, initializer, group),
                    new NettyPacketEncoder(version, cipherAES, cipherIG)
                );
            }))
            .BindAsync(port);

        return new NettyTransportContext(channel, group, version);
    }
}
