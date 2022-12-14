using DotNetty.Transport.Channels;
using Edelstein.Common.Network.DotNetty.Messaging;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler : ChannelHandlerAdapter
{
    private readonly ITransportAcceptor _acceptor;
    private readonly INetworkAdapterInitializer _initializer;

    public NettyTransportAcceptorHandler(ITransportAcceptor acceptor, INetworkAdapterInitializer initializer)
    {
        _acceptor = acceptor;
        _initializer = initializer;
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
        var handshake = new PacketWriter();

        handshake.WriteShort(_acceptor.Version);
        handshake.WriteString(_acceptor.Patch);
        handshake.WriteInt((int)newSocket.SeqRecv);
        handshake.WriteInt((int)newSocket.SeqSend);
        handshake.WriteByte(_acceptor.Locale);
        handshake.WriteBool(false);

        _ = newSocket.Dispatch(handshake);

        context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
        context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

        _acceptor.Sockets.Insert(newSocket);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;
        _acceptor.Sockets.Delete(adapter.Socket.ID);
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnPacket((IPacket)message);
    }
}
