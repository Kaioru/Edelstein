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

public class NettyTransportConnector : ITransportConnector
{
    private readonly IAdapterInitializer _initializer;
    private readonly IRepository<string, ISocket> _sockets;
    private readonly TransportVersion _version;

    public NettyTransportConnector(IAdapterInitializer initializer, TransportVersion version)
    {
        _initializer = initializer;
        _version = version;
        _sockets = new Repository<string, ISocket>();
    }

    public async Task<ITransportContext> Connect(string host, int port)
    {
        var aesCipher = new AESCipher();
        var igCipher = new IGCipher();

        var group0 = new MultithreadEventLoopGroup();
        var channel = await new Bootstrap()
            .Group(group0)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new NettyPacketDecoder(_version, aesCipher, igCipher),
                    new NettyTransportConnectorHandler(_version, _initializer, _sockets),
                    new NettyPacketEncoder(_version, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);

        return new NettyTransportConnectorState(channel, group0, _version, _sockets);
    }
}
