using System;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Contracts;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler<TSocketUser>(
    TransportVersion version, 
    ISocketUserAdapter<TSocketUser> adapter,
    ISocketUserInitializer<TSocketUser> initializer, 
    IChannelGroup group
) : ChannelHandlerAdapter
    where TSocketUser : class, ISocketUser
{
    private readonly AttributeKey<TSocketUser> _userKey = AttributeKey<TSocketUser>.ValueOf("User");

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        using var packet = (IRawPacket)message;

        if (user != null)
        {
            adapter.OnPacket(user, packet);
        }
        else
        {
            using var reader = new RawPacketReader(packet);
            var init = reader.ReadStructured<InitPacket>();

            if (init.Version != version.Major) return;
            if (init.Patch.Value != version.Patch) return;
            if (init.Locale != version.Locale) return;

            var newSocket = new NettySocket(
                context.Channel,
                version,
                init.SeqSend,
                init.SeqRecv
            );
            var newUser = initializer.Initialize(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(_userKey).Set(newUser);

            group.Add(context.Channel);
        }
    }
    
    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();

        adapter.OnDisconnect(user);
        group.Remove(context.Channel);
        base.ChannelInactive(context);
    }


    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        
        if (user == null) return;
        
        adapter.OnException(user, exception);
    }
}
