using System;
using DotNetty.Transport.Channels;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Transport.Channels
{
    public class ServerAdapter : ChannelHandlerAdapter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly Server _server;
        private readonly ISocketFactory _socketFactory;

        public ServerAdapter(
            Server server,
            ISocketFactory socketFactory
        )
        {
            _server = server;
            _socketFactory = socketFactory;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var random = new Random();
            var socket = _socketFactory.Build(
                context.Channel,
                (uint) random.Next(),
                (uint) random.Next()
            );

            using (var p = new Packet())
            {
                p.Encode<short>(95);
                p.Encode<string>("1");
                p.Encode<int>((int) socket.SeqRecv);
                p.Encode<int>((int) socket.SeqSend);
                p.Encode<byte>(8);

                socket.SendPacket(p);
            }

            context.Channel.GetAttribute(AbstractSocket.SocketKey).Set(socket);
            Logger.Debug($"Accepted connection from {context.Channel.RemoteAddress}");

            lock (_server) _server.Sockets.Add(socket);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();

            socket?.OnDisconnect();
            base.ChannelInactive(context);
            Logger.Debug($"Released connection from {context.Channel.RemoteAddress}");

            lock (_server) _server.Sockets.Remove(socket);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();
            socket?.OnPacket((Packet) message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();
            socket?.OnException(exception);
        }
    }
}