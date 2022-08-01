using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Protocol.Network;

public interface IAdapter
{
    ISocket Socket { get; }

    Task OnPacket(IByteBuffer packet);
    Task OnException(Exception exception);
    Task OnDisconnect();

    Task Dispatch(IByteBuffer packet);
    Task Disconnect();
}
