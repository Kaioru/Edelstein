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

public class NettyTransportAcceptor : ITransportAcceptor
{
    private readonly IAdapterInitializer _initializer;
    private readonly IRepository<string, ISocket> _sockets;
    private readonly TransportVersion _version;

    public NettyTransportAcceptor(IAdapterInitializer initializer, TransportVersion version)
    {
        _initializer = initializer;
        _version = version;
        _sockets = new Repository<string, ISocket>();
    }

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
                    new NettyPacketDecoder(_version, aesCipher, igCipher),
                    new NettyTransportAcceptorHandler(_version, _initializer, _sockets),
                    new NettyPacketEncoder(_version, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);

        return new NettyTransportAcceptorState(channel, group0, group1, _version, _sockets);
    }
}
