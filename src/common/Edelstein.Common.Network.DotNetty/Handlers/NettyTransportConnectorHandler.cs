using System;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Buffers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler<TSocketUser>(
    TransportVersion transportVersion,
    ISocketUserCreator<TSocketUser> creator,
    ISocketAdapter<TSocketUser> socketAdapter,
    IRepository<string, ISocket> sockets
) : ChannelHandlerAdapter
    where TSocketUser : class, ISocketUser
{
    private readonly AttributeKey<TSocketUser> _userKey = AttributeKey<TSocketUser>.ValueOf("User");
    
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        using var packet = (IPacket)message;

        if (user != null)
        {
            socketAdapter.OnPacket(user, packet);
        }
        else
        {
            using var reader = new PacketReader(packet);
            var version = reader.ReadShort();
            var patch = reader.ReadString();
            var seqSend = reader.ReadUInt();
            var seqRecv = reader.ReadUInt();
            var locale = reader.ReadByte();

            if (version != transportVersion.Major) return;
            if (patch != transportVersion.Patch) return;
            if (locale != transportVersion.Locale) return;

            var newSocket = new NettySocket(
                context.Channel,
                seqSend,
                seqRecv
            );
            var newAdapter = creator.CreateUser(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(_userKey).Set(newAdapter);

            _ = sockets.Insert(newSocket);
        }
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();

        socketAdapter.OnDisconnect(user);
        base.ChannelInactive(context);

        if (user == null) return;

        _ = sockets.Delete(user.Socket);
    }


    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var user = context.Channel.GetAttribute(_userKey).Get();
        
        if (user == null) return;
        
        socketAdapter.OnException(user, exception);
    }
}
