using System;
using DotNetty.Transport.Channels;
using Edelstein.Common.Network.DotNetty.Transport;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Pipeline
{
    public class NettyTransportAcceptorHandler : ChannelHandlerAdapter
    {
        private readonly ITransportAcceptor _acceptor;

        public NettyTransportAcceptorHandler(ITransportAcceptor acceptor)
        {
            _acceptor = acceptor;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var random = new Random();
            var newSocket = new NettySocket(
                context.Channel,
                (uint)random.Next(),
                (uint)random.Next()
            );
            var newSession = _acceptor.SessionInitializer.Initialize(newSocket);
            var handshake = new UnstructuredOutgoingPacket();

            handshake.WriteShort(_acceptor.Version);
            handshake.WriteString(_acceptor.Patch);
            handshake.WriteInt((int)newSocket.SeqRecv);
            handshake.WriteInt((int)newSocket.SeqSend);
            handshake.WriteByte(_acceptor.Locale);

            _ = newSocket.Dispatch(handshake);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(NettyAttributes.SessionKey).Set(newSession);

            lock (_acceptor) _acceptor.Sessions.Add(newSession.ID, newSession);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();

            session?.OnDisconnect();
            base.ChannelInactive(context);

            lock (_acceptor) _acceptor.Sessions.Remove(session.ID);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();

            session?.OnPacket((IPacketReader)message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();

            session?.OnException(exception);
        }
    }
}