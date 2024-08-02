using System;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Contracts;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler<TSocketUser>(
    TransportVersion version, 
    ISocketUserAdapter<TSocketUser> adapter,
    ISocketUserInitializer<TSocketUser> initializer, 
    IChannelGroup group
) : ChannelHandlerAdapter
    where TSocketUser : class, ISocketUser
{
    private readonly AttributeKey<TSocketUser> _userKey = AttributeKey<TSocketUser>.ValueOf("User");
    
    public override void ChannelActive(IChannelHandlerContext context)
    {
        var newSocket = new NettySocket(
            context.Channel,
            version,
            (uint)Random.Shared.Next(),
            (uint)Random.Shared.Next()
        );
        var newUser = initializer.Initialize(newSocket);
        
        _ = newSocket.Dispatch(new InitPacket
        {
            Version = version.Major,
            Patch = new LPString(version.Patch),
            SeqRecv = newSocket.SeqRecv,
            SeqSend = newSocket.SeqSend,
            Locale = version.Locale
        });
        
        context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
        context.Channel.GetAttribute(_userKey).Set(newUser);

        group.Add(context.Channel);
    }
    
    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();

        adapter.OnDisconnect(user);
        group.Remove(context.Channel);
        base.ChannelInactive(context);
    }
    
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        using var packet = (IRawPacket)message;
        
        if (user == null) return;
        
        adapter.OnPacket(user, packet);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        
        if (user == null) return;
        
        adapter.OnException(user, exception);
    }
}
