using DotNetty.Transport.Channels;
using Edelstein.Common.Util.Buffers.Bytes;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Network.DotNetty.Handlers;

public class NettyTransportAcceptorHandler : ChannelHandlerAdapter
{
    private readonly ITransportAcceptor _acceptor;

    public NettyTransportAcceptorHandler(ITransportAcceptor acceptor) =>
        _acceptor = acceptor ?? throw new ArgumentNullException(nameof(acceptor));

    public override void ChannelActive(IChannelHandlerContext context)
    {
        var random = new Random();
        var newSocket = new NettySocket(
            context.Channel,
            (uint)random.Next(),
            (uint)random.Next()
        );
        var newAdapter = _acceptor.Initializer.Initialize(newSocket);
        var handshake = new PacketWriter();

        handshake.WriteShort(_acceptor.Version);
        handshake.WriteString(_acceptor.Patch);
        handshake.WriteInt((int)newSocket.SeqRecv);
        handshake.WriteInt((int)newSocket.SeqSend);
        handshake.WriteByte(_acceptor.Locale);

        _ = newSocket.Dispatch(handshake);

        context.Channel.GetAttribute(NettyAttributes.SocketKey).Set(newSocket);
        context.Channel.GetAttribute(NettyAttributes.AdapterKey).Set(newAdapter);

        _acceptor.Sockets.Add(newSocket.ID, newSocket);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnDisconnect();
        base.ChannelInactive(context);

        if (adapter == null) return;
        _acceptor.Sockets.Remove(adapter.Socket.ID);
    }

    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnPacket((IPacketReader)message);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        var adapter = context.Channel.GetAttribute(NettyAttributes.AdapterKey).Get();

        adapter?.OnException(exception);
    }
}
