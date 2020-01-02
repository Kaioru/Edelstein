using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Edelstein.Network.Codecs;
using Edelstein.Network.Logging;

namespace Edelstein.Network.Transport
{
    public class Server
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public ISocketAdapterFactory AdapterFactory { get; }
        public short Version { get; }
        public string Patch { get; }
        public byte Locale { get; }

        private IChannel? Channel { get; set; }
        private IEventLoopGroup? BossGroup { get; set; }
        private IEventLoopGroup? WorkerGroup { get; set; }

        public ICollection<ISocket> Sockets { get; }

        public Server(ISocketAdapterFactory adapterFactory, short version, string patch, byte locale)
        {
            AdapterFactory = adapterFactory;
            Version = version;
            Patch = patch;
            Locale = locale;
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
                        new ReadTimeoutHandler(TimeSpan.FromMinutes(1)),
                        new PacketDecoder(Version),
                        new ServerAdapter(this),
                        new PacketEncoder(Version)
                    );
                }))
                .BindAsync(port);

            Logger.Info($"Bounded server on {host}:{port}");
        }

        public async Task Stop()
        {
            await Task.WhenAll(Sockets.Select(s => s.Close()));
            await Channel.CloseAsync();
            await BossGroup.ShutdownGracefullyAsync();
            await WorkerGroup.ShutdownGracefullyAsync();
        }
    }
}