using System;
using System.Threading.Tasks;
using DotNetty.Common.Concurrency;
using DotNetty.Handlers.Timeout;
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

public class NettyTransportAcceptor<TSocketUser>(
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
        var group1 = new MultithreadEventLoopGroup();
        var channel = await new ServerBootstrap()
            .Group(group0, group1)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 1024)
            .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new ReadTimeoutHandler(TimeSpan.FromMinutes(5)),
                    new NettyPacketDecoder(version, cipherAES, cipherIG),
                    new NettyTransportAcceptorHandler<TSocketUser>(version, adapter, initializer, group),
                    new NettyPacketEncoder(version, cipherAES, cipherIG)
                );
            }))
            .BindAsync(port);
        
        return new NettyTransportContext(channel, group, version);
    }
}
