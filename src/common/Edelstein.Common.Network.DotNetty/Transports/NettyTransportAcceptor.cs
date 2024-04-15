using System;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Crypto;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportAcceptor<TSocketUser>(
    TransportVersion version,
    ISocketUserCreator<TSocketUser> creator, 
    ISocketAdapter<TSocketUser> socketAdapter
) : ITransportAcceptor 
    where TSocketUser : class, ISocketUser
{
    private readonly IRepository<string, ISocket> _sockets = new Repository<string, ISocket>();

    public async Task<ITransportContext> Accept(string host, int port)
    {
        var aesCipher = new AESCipher();
        var igCipher = new IGCipher();

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
                    new NettyPacketDecoder(version, aesCipher, igCipher),
                    new NettyTransportAcceptorHandler<TSocketUser>(version, creator, socketAdapter, _sockets),
                    new NettyPacketEncoder(version, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);

        return new NettyTransportAcceptorState(channel, group0, group1, version, _sockets);
    }
}
