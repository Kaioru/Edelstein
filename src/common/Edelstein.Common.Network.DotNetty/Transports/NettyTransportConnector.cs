using System.Threading.Tasks;
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

public class NettyTransportConnector<TSocketUser>(
    TransportVersion version,
    ISocketUserCreator<TSocketUser> creator,
    ISocketAdapter<TSocketUser> socketAdapter
) : ITransportConnector
    where TSocketUser : class, ISocketUser
{
    private readonly IRepository<string, ISocket> _sockets = new Repository<string, ISocket>();

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
                    new NettyPacketDecoder(version, aesCipher, igCipher),
                    new NettyTransportConnectorHandler<TSocketUser>(version, creator, socketAdapter, _sockets),
                    new NettyPacketEncoder(version, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);

        return new NettyTransportConnectorState(channel, group0, version, _sockets);
    }
}
