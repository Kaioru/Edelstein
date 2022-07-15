using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler : ChannelHandlerAdapter
{
    private readonly ITransportConnector _connector;

    public NettyTransportConnectorHandler(ITransportConnector connector)
    {
        _connector = connector ?? throw new ArgumentNullException(nameof(connector));
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        var packet = (IPacketReader)message;

        if (adapter != null) adapter.OnPacket(packet);
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
            var newAdapter = _connector.Initializer.Initialize(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);
        }
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);
    }


    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnException(exception);
    }
}
