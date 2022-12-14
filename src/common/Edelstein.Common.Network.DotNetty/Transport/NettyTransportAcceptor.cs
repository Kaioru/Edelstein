using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Cryptography;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Common.Util.Storages;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Network.Transport;
using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Common.Network.DotNetty.Transport;

public class NettyTransportAcceptor : ITransportAcceptor
{
    private readonly INetworkAdapterInitializer _initializer;

    public TransportState State { get; set; }

    public short Version { get; }
    public string Patch { get; }
    public byte Locale { get; }

    public IStorage<string, ISocket> Sockets { get; }

    private IChannel? Channel { get; set; }
    private IEventLoopGroup? BossGroup { get; set; }
    private IEventLoopGroup? WorkerGroup { get; set; }

    public NettyTransportAcceptor(INetworkAdapterInitializer initializer, short version, string patch, byte locale)
    {
        _initializer = initializer;
        Version = version;
        Patch = patch;
        Locale = locale;
        Sockets = new Storage<string, ISocket>();
    }

    public async Task Accept(string host, int port)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));
        var aesCipher = new AESCipher();
        var igCipher = new IGCipher();

        BossGroup = new MultithreadEventLoopGroup();
        WorkerGroup = new MultithreadEventLoopGroup();
        Channel = await new ServerBootstrap()
            .Group(BossGroup, WorkerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 1024)
            .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
            {
                ch.Pipeline.AddLast(
                    new ReadTimeoutHandler(TimeSpan.FromMinutes(4)),
                    new NettyPacketDecoder(this, aesCipher, igCipher),
                    new NettyTransportAcceptorHandler(this, _initializer),
                    new NettyPacketEncoder(this, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);

        State = TransportState.Active;
    }

    public Task Dispatch(IPacket packet) => Task.WhenAll(Sockets.RetrieveAll().Select(s => s.Dispatch(packet)));

    public async Task Close()
    {
        await Task.WhenAll(Sockets.RetrieveAll().Select(s => s.Close()));

        if (Channel != null) await Channel.CloseAsync();

        await Task.WhenAll(
            Task.Run(async () =>
            {
                if (BossGroup != null)
                    await BossGroup.ShutdownGracefullyAsync(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            }),
            Task.Run(async () =>
            {
                if (WorkerGroup != null)
                    await WorkerGroup.ShutdownGracefullyAsync(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            })
        );

        State = TransportState.Idle;
    }
}
