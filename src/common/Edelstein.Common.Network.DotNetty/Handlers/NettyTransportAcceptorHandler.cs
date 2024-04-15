using System;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Buffers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler<TSocketUser>(
    TransportVersion version, 
    ISocketUserCreator<TSocketUser> creator, 
    ISocketAdapter<TSocketUser> socketAdapter,
    IRepository<string, ISocket> sockets
) : ChannelHandlerAdapter
    where TSocketUser : class, ISocketUser
{
    private readonly AttributeKey<TSocketUser> _userKey = AttributeKey<TSocketUser>.ValueOf("User");

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var random = new Random();
        var newSocket = new NettySocket(
            context.Channel,
            (uint)random.Next(),
            (uint)random.Next()
        );
        var newUser = creator.CreateUser(newSocket);
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
        context.Channel.GetAttribute(_userKey).Set(newUser);

        _ = sockets.Insert(newSocket);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();

        socketAdapter.OnDisconnect(user);
        base.ChannelInactive(context);

        if (user == null) return;

        _ = sockets.Delete(user.Socket);
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        using var packet = (IPacket)message;
        
        if (user == null) return;
        
        socketAdapter.OnPacket(user, packet);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        
        if (user == null) return;
        
        socketAdapter.OnException(user, exception);
    }
}
