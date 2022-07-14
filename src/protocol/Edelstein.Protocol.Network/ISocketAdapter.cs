using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network;

public interface ISocketAdapter
{
    ISocket Socket { get; }

    Task OnPacket(IPacket packet);
    Task OnException(Exception exception);
    Task OnDisconnect();

    Task Dispatch(IPacket packet);
    Task Disconnect();
}
