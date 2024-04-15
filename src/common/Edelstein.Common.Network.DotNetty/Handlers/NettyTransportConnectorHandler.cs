using System;
using DotNetty.Transport.Channels;
using Edelstein.Common.Utilities.Buffers;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportConnectorHandler(
    TransportVersion transportVersion,
    IAdapterInitializer initializer, 
    IRepository<string, ISocket> sockets
) : ChannelHandlerAdapter
{

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();
        using var packet = (IPacket)message;

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

            if (version != transportVersion.Major) return;
            if (patch != transportVersion.Patch) return;
            if (locale != transportVersion.Locale) return;

            var newSocket = new NettySocket(
                context.Channel,
                seqSend,
                seqRecv
            );
            var newAdapter = initializer.Initialize(newSocket);

            context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
            context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

            _ = sockets.Insert(newSocket);
        }
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;

        _ = sockets.Delete(adapter.Socket);
    }


    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnException(exception);
    }
}
