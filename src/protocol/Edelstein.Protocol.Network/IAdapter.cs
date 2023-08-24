using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Network;

public interface IAdapter
{
    ISocket Socket { get; }

    Task OnPacket(IPacket packet);
    Task OnException(Exception exception);
    Task OnDisconnect();

    Task Dispatch(IPacket packet);
    Task Disconnect();
}
