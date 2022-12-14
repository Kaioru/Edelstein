using System.Net;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Cryptography;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Transport;

public class NettyTransportConnector : ITransportConnector
{
    private readonly INetworkAdapterInitializer _initializer;

    public TransportState State { get; set; }

    public short Version { get; }
    public string Patch { get; }
    public byte Locale { get; }

    public ISocket? Socket { get; set; }

    private IEventLoopGroup? WorkerGroup { get; set; }

    public NettyTransportConnector(INetworkAdapterInitializer initializer, short version, string patch, byte locale)
    {
        _initializer = initializer;
        Version = version;
        Patch = patch;
        Locale = locale;
    }

    public async Task Connect(string host, int port)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));
        var aesCipher = new AESCipher();
        var igCipher = new IGCipher();

        WorkerGroup = new MultithreadEventLoopGroup();

        await new Bootstrap()
            .Group(WorkerGroup)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new NettyPacketDecoder(this, aesCipher, igCipher),
                    new NettyTransportConnectorHandler(this, _initializer),
                    new NettyPacketEncoder(this, aesCipher, igCipher)
                );
            }))
            .ConnectAsync(IPAddress.Parse(host), port);

        State = TransportState.Active;
    }

    public Task Dispatch(IPacket packet) => Task.FromResult(Socket?.Dispatch(packet));

    public async Task Close()
    {
        if (Socket != null) await Socket.Close();
        if (WorkerGroup != null) await WorkerGroup.ShutdownGracefullyAsync();

        State = TransportState.Idle;
    }
}
