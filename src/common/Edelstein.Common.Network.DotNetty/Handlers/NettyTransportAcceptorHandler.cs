using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler : ChannelHandlerAdapter
{
    private readonly IAdapterInitializer _initializer;
    private readonly IRepository<string, ISocket> _sockets;
    private readonly TransportVersion _version;

    public NettyTransportAcceptorHandler(TransportVersion version, IAdapterInitializer initializer, IRepository<string, ISocket> sockets)
    {
        _version = version;
        _initializer = initializer;
        _sockets = sockets;
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var random = new Random();
        var newSocket = new NettySocket(
            context.Channel,
            (uint)random.Next(),
            (uint)random.Next()
        );
        var newAdapter = _initializer.Initialize(newSocket);
        using var handshake = new PacketWriter();

        handshake.WriteShort(_version.Major);
        handshake.WriteString(_version.Patch);
        handshake.WriteInt((int)newSocket.SeqRecv);
        handshake.WriteInt((int)newSocket.SeqSend);
        handshake.WriteByte(_version.Locale);

        _ = newSocket.Dispatch(
            new PacketWriter()
                .WriteShort(_version.Major)
                .WriteString(_version.Patch)
                .WriteInt((int)newSocket.SeqRecv)
                .WriteInt((int)newSocket.SeqSend)
                .WriteByte(_version.Locale)
                .Build()
        );

        context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
        context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

        _ = _sockets.Insert(newSocket);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;

        _ = _sockets.Delete(adapter.Socket);
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        adapter?.OnPacket((IPacket)message);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnException(exception);
    }
}
