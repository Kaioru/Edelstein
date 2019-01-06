using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Logging;
using Edelstein.Network.Packet.Codecs;

namespace Edelstein.Network
{
    public class Server : IServer
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        private readonly ISocketFactory _socketFactory;
        private IChannel Channel { get; set; }
        private IEventLoopGroup BossGroup { get; set; }
        private IEventLoopGroup WorkerGroup { get; set; }

        public ICollection<ISocket> Sockets { get; }

        public Server(ISocketFactory socketFactory)
        {
            _socketFactory = socketFactory;
            Sockets = new List<ISocket>();
        }

        public async Task Start(string host, int port)
        {
            BossGroup = new MultithreadEventLoopGroup();
            WorkerGroup = new MultithreadEventLoopGroup();

            Channel = await new ServerBootstrap()
                .Group(BossGroup, WorkerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 1024)
                .ChildHandler(new ActionChannelInitializer<IChannel>(ch =>
                {
                    ch.Pipeline.AddLast(
                        new PacketDecoder(),
                        new ServerAdapter(this, _socketFactory),
                        new PacketEncoder()
                    );
                }))
                .BindAsync(IPAddress.Parse(host), port);
            
            Logger.Info($"Bounded server on {host}:{port}");
            await Channel.CloseCompletion;
        }

        public async Task Stop()
        {
            await Channel.CloseAsync();
            await BossGroup.ShutdownGracefullyAsync();
            await WorkerGroup.ShutdownGracefullyAsync();
        }
    }
}