using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Packets.Codecs;

namespace Edelstein.Network
{
    public class Client : IClient
    {
        private readonly ISocketFactory _socketFactory;
        private IChannel Channel { get; set; }
        private IEventLoopGroup WorkerGroup { get; set; }

        public ISocket Socket { get; set; }

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
        }

        public async Task Stop()
        {
            await WorkerGroup.ShutdownGracefullyAsync();
            await Channel.CloseAsync();
        }
    }
}