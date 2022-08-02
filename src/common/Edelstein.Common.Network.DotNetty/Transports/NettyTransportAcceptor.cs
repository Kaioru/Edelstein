using System.Collections.Concurrent;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Crypto;
using Edelstein.Common.Network.DotNetty.Codecs;
using Edelstein.Common.Network.DotNetty.Handlers;
using Edelstein.Common.Util.Buffers.Bytes;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportAcceptor : ITransportAcceptor
{
    public NettyTransportAcceptor(IAdapterInitializer initializer, short version, string patch, byte locale)
    {
        Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        Sockets = new ConcurrentDictionary<string, ISocket>();
        Version = version;
        Patch = patch;
        Locale = locale;
    }

    private IChannel? Channel { get; set; }
    private IEventLoopGroup? BossGroup { get; set; }
    private IEventLoopGroup? WorkerGroup { get; set; }
    public IAdapterInitializer Initializer { get; }
    public IDictionary<string, ISocket> Sockets { get; }

    public short Version { get; }
    public string Patch { get; }
    public byte Locale { get; }

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
                    new NettyTransportAcceptorHandler(this),
                    new NettyPacketEncoder(this, aesCipher, igCipher)
                );
            }))
            .BindAsync(port);
    }

    public Task Dispatch(IPacket packet) =>
        Task.FromResult(Sockets.Values.Select(s => s.Dispatch(new PacketReader(packet.Buffer))));

    public async Task Close()
    {
        await Task.WhenAll(Sockets.Values.Select(s => s.Close()));
        if (Channel != null) await Channel.CloseAsync();
        await Task.WhenAll(
            Task.Run(async () =>
            {
                if (BossGroup != null)
                    await BossGroup.ShutdownGracefullyAsync();
            }),
            Task.Run(async () =>
            {
                if (WorkerGroup != null)
                    await WorkerGroup.ShutdownGracefullyAsync();
            })
        );
    }
}
