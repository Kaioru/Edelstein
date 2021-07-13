using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Network.DotNetty.Pipeline;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Ciphers;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Transport
{
    public class NettyTransportConnector : ITransportConnector
    {
        public ISession Session { get; set; }
        public ISessionInitializer SessionInitializer { get; init; }

        public short Version { get; init; }
        public string Patch { get; init; }
        public byte Locale { get; init; }

        private IChannel Channel { get; set; }
        private IEventLoopGroup WorkerGroup { get; set; }

        public NettyTransportConnector(
            ISessionInitializer initializer,
            short version,
            string patch,
            byte locale
        )
        {
            SessionInitializer = initializer;
            Version = version;
            Patch = patch;
            Locale = locale;
        }

        public async Task Connect(string host, int port)
        {
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

        public Task Dispatch(IPacket packet)
            => Session?.Dispatch(packet);

        public Task Dispatch(IEnumerable<IPacket> packets)
            => Session?.Dispatch(packets);

        public async Task Close()
        {
            if (Session != null) await Session.Disconnect();
            if (Channel != null) await Channel.CloseAsync();
            if (WorkerGroup != null) await WorkerGroup.ShutdownGracefullyAsync();
        }
    }
}
