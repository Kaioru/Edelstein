using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets.Codecs;
using Edelstein.Network.Transport.Channels;

namespace Edelstein.Network.Transport
{
    public class Client
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly ISocketFactory _socketFactory;

        private IChannel Channel { get; set; }
        private IEventLoopGroup WorkerGroup { get; set; }
        public ISocket Socket { get; set; }

        public Client(ISocketFactory socketFactory)
        {
            _socketFactory = socketFactory;
        }

        public async Task Start(string host, int port)
        {
            WorkerGroup = new MultithreadEventLoopGroup();
            Channel = await new Bootstrap()
                .Group(WorkerGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        new ClientAdapter(this, _socketFactory),
                        new PacketEncoder()
                    );
                }))
                .ConnectAsync(IPAddress.Parse(host), port);

            Logger.Info($"Established connection to {host}:{port}");
        }

        public async Task Stop()
        {
            await Channel.CloseAsync();
            await WorkerGroup.ShutdownGracefullyAsync();
        }
    }
}