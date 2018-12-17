using System;
using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class ClientAdapter : ChannelHandlerAdapter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly Client _client;
        private readonly ISocketFactory _socketFactory;

        public ClientAdapter(
            Client client,
            ISocketFactory socketFactory
        )
        {
            _client = client;
            _socketFactory = socketFactory;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();
            var p = (Packet) message;

            if (socket != null) socket.OnPacket(p);
            else
            {
                var version = p.Decode<short>();

                p.Decode<string>();

                var seqSend = p.Decode<uint>();
                var seqRecv = p.Decode<uint>();

                p.Decode<byte>();

                if (version != AESCipher.Version) return;

                var newSocket = _socketFactory.Build(
                    context.Channel,
                    seqSend,
                    seqRecv
                );

                lock (_client)
                {
                    if (_client.Socket == null)
                        _client.Socket = newSocket;
                }

                context.Channel.GetAttribute(AbstractSocket.SocketKey).Set(newSocket);
                Logger.Debug($"Established connection to {context.Channel.RemoteAddress}");
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();

            socket?.OnDisconnect();
            base.ChannelInactive(context);

            Logger.Debug($"Released connection to {context.Channel.RemoteAddress}");
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();
            socket?.OnException(exception);
        }
    }
}