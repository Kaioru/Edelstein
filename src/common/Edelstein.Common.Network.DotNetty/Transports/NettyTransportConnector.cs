using System.Net;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Crypto;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportConnector : ITransportConnector
{
    public NettyTransportConnector(IAdapterInitializer initializer, short version, string patch, byte locale)
    {
        Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        Version = version;
        Patch = patch;
        Locale = locale;
    }

    private IChannel? Channel { get; set; }
    private IEventLoopGroup? WorkerGroup { get; set; }
    public IAdapterInitializer Initializer { get; }

    public short Version { get; }
    public string Patch { get; }
    public byte Locale { get; }

    public async Task Connect(string host, int port)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));
        var aesCipher = new AESCipher();
        var igCipher = new IGCipher();

        WorkerGroup = new MultithreadEventLoopGroup();
        Channel = await new Bootstrap()
            .Group(WorkerGroup)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new NettyPacketDecoder(this, aesCipher, igCipher),
                    new NettyTransportConnectorHandler(this),
                    new NettyPacketEncoder(this, aesCipher, igCipher)
                );
            }))
            .ConnectAsync(IPAddress.Parse(host), port);
    }

    public Task Dispatch(IPacket packet) => Task.FromResult(Channel?.WriteAndFlushAsync(packet));

    public async Task Close()
    {
        if (Channel != null) await Channel.CloseAsync();
        if (WorkerGroup != null) await WorkerGroup.ShutdownGracefullyAsync();
    }
}
