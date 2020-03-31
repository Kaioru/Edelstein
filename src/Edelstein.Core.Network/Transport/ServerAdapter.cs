using System;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Transport
{
    public class ServerAdapter : ChannelHandlerAdapter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly Server _server;

        public ServerAdapter(Server server)
            => _server = server;

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var random = new Random();
            var socket = new Socket(
                context.Channel,
                (uint) random.Next(),
                (uint) random.Next()
            );
            var adapter = _server.AdapterFactory.Build(socket);

            using (var p = new Packet())
            {
                p.EncodeShort(_server.Version);
                p.EncodeString(_server.Patch);
                p.EncodeInt((int) socket.SeqRecv);
                p.EncodeInt((int) socket.SeqSend);
                p.EncodeByte(_server.Locale);

                socket.SendPacket(p);
            }

            context.Channel.GetAttribute(Socket.Key).Set(socket);
            context.Channel.GetAttribute(AbstractSocketAdapter.Key).Set(adapter);
            Logger.Debug($"Accepted connection from {context.Channel.RemoteAddress}");

            lock (_server) _server.Sockets.Add(socket);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var socket = context.Channel.GetAttribute(Socket.Key).Get();
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();

            adapter?.OnDisconnect();
            base.ChannelInactive(context);
            Logger.Debug($"Released connection from {context.Channel.RemoteAddress}");

            lock (_server) _server.Sockets.Remove(socket);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();
            adapter?.OnPacket((IPacket) message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();

            if (exception is ReadTimeoutException)
                Logger.Debug($"Closing connection from {context.Channel.RemoteAddress} due to idle activity");
            else adapter?.OnException(exception);
        }
    }
}