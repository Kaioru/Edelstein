using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Common.Network.DotNetty.Pipeline;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Ciphers;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Edelstein.Common.Network.DotNetty.Transport
{
    public class NettyTransportAcceptor : ITransportAcceptor
    {
        public IDictionary<string, ISession> Sessions { get; init; }
        public ISessionInitializer SessionInitializer { get; init; }

        public short Version { get; init; }
        public string Patch { get; init; }
        public byte Locale { get; init; }

        private IChannel Channel { get; set; }
        private IEventLoopGroup BossGroup { get; set; }
        private IEventLoopGroup WorkerGroup { get; set; }

        private readonly ILogger<ITransportAcceptor> _logger;

        public NettyTransportAcceptor(
            ISessionInitializer initializer,
            short version,
            string patch,
            byte locale,
            ILogger<ITransportAcceptor> logger = null
        )
        {
            Sessions = new Dictionary<string, ISession>();
            SessionInitializer = initializer;
            Version = version;
            Patch = patch;
            Locale = locale;

            _logger = logger ?? NullLogger<ITransportAcceptor>.Instance;
        }

        public async Task Accept(string host, int port)
        {
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
                        new ReadTimeoutHandler(TimeSpan.FromMinutes(1)),
                        new NettyPacketDecoder(this, aesCipher, igCipher),
                        new NettyTransportAcceptorHandler(this),
                        new NettyPacketEncoder(this, aesCipher, igCipher)
                    );
                }))
                .BindAsync(port);

            _logger.LogInformation($"Socket acceptor bound on {host}:{port}");
        }

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(Sessions.Values.Select(s => s.Dispatch(packet)));

        public Task Dispatch(IEnumerable<IPacket> packets)
            => Task.WhenAll(Sessions.Values.Select(s => s.Dispatch(packets)));

        public async Task Close()
        {
            await Task.WhenAll(Sessions.Values.Select(s => s.Disconnect()));
            if (Channel != null) await Channel.CloseAsync();
            if (BossGroup != null) await BossGroup.ShutdownGracefullyAsync();
            if (WorkerGroup != null) await WorkerGroup.ShutdownGracefullyAsync();
        }
    }
}
