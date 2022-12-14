using Edelstein.Protocol.Network.Messaging;

namespace Edelstein.Protocol.Network;

public interface INetworkAdapter
{
    ISocket Socket { get; }

    Task OnPacket(IPacket packet);
    Task OnDisconnect();

    Task Dispatch(IPacket packet);
    Task Disconnect();
}
