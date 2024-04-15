using System;
using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Buffers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler(
    TransportVersion version, 
    IAdapterInitializer initializer, 
    IRepository<string, ISocket> sockets
) : ChannelHandlerAdapter
{

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var random = new Random();
        var newSocket = new NettySocket(
            context.Channel,
            (uint)random.Next(),
            (uint)random.Next()
        );
        var newAdapter = initializer.Initialize(newSocket);
        using var handshake = new PacketWriter();

        handshake.WriteShort(version.Major);
        handshake.WriteString(version.Patch);
        handshake.WriteInt((int)newSocket.SeqRecv);
        handshake.WriteInt((int)newSocket.SeqSend);
        handshake.WriteByte(version.Locale);

        var packet = new PacketWriter()
            .WriteShort(version.Major)
            .WriteString(version.Patch)
            .WriteInt((int)newSocket.SeqRecv)
            .WriteInt((int)newSocket.SeqSend)
            .WriteByte(version.Locale);

        _ = newSocket.Dispatch(packet.Build());

        context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
        context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

        _ = sockets.Insert(newSocket);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;

        _ = sockets.Delete(adapter.Socket);
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        using var packet = (IPacket)message;
        
        adapter?.OnPacket(packet);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        adapter?.OnException(exception);
    }
}
