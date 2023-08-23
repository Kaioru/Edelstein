using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler : ChannelHandlerAdapter
{
    private readonly IAdapterInitializer _initializer;
    private readonly IRepository<string, ISocket> _sockets;
    private readonly TransportVersion _version;

    public NettyTransportConnectorHandler(TransportVersion version, IAdapterInitializer initializer, IRepository<string, ISocket> sockets)
    {
        _version = version;
        _initializer = initializer;
        _sockets = sockets;
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
            using var reader = new PacketReader(packet);
            var version = reader.ReadShort();
            var patch = reader.ReadString();
            var seqSend = reader.ReadUInt();
            var seqRecv = reader.ReadUInt();
            var locale = reader.ReadByte();

            if (version != _version.Major) return;
            if (patch != _version.Patch) return;
            if (locale != _version.Locale) return;

            var newSocket = new NettySocket(
                context.Channel,
                seqSend,
                seqRecv
            );
            var newAdapter = _initializer.Initialize(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

            _ = _sockets.Insert(newSocket);
        }
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;

        _ = _sockets.Delete(adapter.Socket);
    }


    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnException(exception);
    }
}
