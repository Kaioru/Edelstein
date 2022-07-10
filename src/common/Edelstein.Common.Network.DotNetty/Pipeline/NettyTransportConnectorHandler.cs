using System;
using System.Diagnostics;
using DotNetty.Transport.Channels;
using Edelstein.Common.Network.DotNetty.Transport;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Pipeline
{
    public class NettyTransportConnectorHandler : ChannelHandlerAdapter
    {
        private readonly ITransportConnector _connector;

        public NettyTransportConnectorHandler(ITransportConnector connector)
        {
            Debug.Assert(connector != null);

            _connector = connector;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();
            var packet = (IPacketReader)message;

            if (session != null) session.OnPacket(packet);
            else
            {
                var version = packet.ReadShort();
                var patch = packet.ReadString();
                var seqSend = packet.ReadUInt();
                var seqRecv = packet.ReadUInt();
                var locale = packet.ReadByte();

                if (version != _connector.Version) return;
                if (patch != _connector.Patch) return;
                if (locale != _connector.Locale) return;

                var newSocket = new NettySocket(
                    context.Channel,
                    seqSend,
                    seqRecv
                );
                var newSession = _connector.SessionInitializer.Initialize(newSocket);

                lock (_connector)
                    _connector.Session = newSession;

                context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
                context.Channel.GetAttribute(NettyAttributes.SessionKey).Set(newSession);
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();

            session?.OnDisconnect();
            base.ChannelInactive(context);
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var session = context.Channel.GetAttribute(NettyAttributes.SessionKey).Get();

            session?.OnException(exception);
        }
    }
}