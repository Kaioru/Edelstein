using DotNetty.Transport.Channels;
using Edelstein.Common.Network.DotNetty.Messaging;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler : ChannelHandlerAdapter
{
    private readonly ITransportConnector _connector;
    private readonly INetworkAdapterInitializer _initializer;

    public NettyTransportConnectorHandler(ITransportConnector connector, INetworkAdapterInitializer initializer)
    {
        _connector = connector;
        _initializer = initializer;
    }


    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        var packet = (IPacket)message;

        if (adapter != null)
        {
            adapter.OnPacket(packet);
        }
        else
        {
            var reader = new PacketReader(packet.Buffer);
            var version = reader.ReadShort();
            var patch = reader.ReadString();
            var seqSend = reader.ReadUInt();
            var seqRecv = reader.ReadUInt();
            var locale = reader.ReadByte();

            if (version != _connector.Version) return;
            if (patch != _connector.Patch) return;
            if (locale != _connector.Locale) return;

            var newSocket = new NettySocket(
                context.Channel,
                seqSend,
                seqRecv
            );
            var newAdapter = _initializer.Initialize(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

            _connector.Socket = newSocket;
        }
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        _connector.Socket = null;
    }
}
