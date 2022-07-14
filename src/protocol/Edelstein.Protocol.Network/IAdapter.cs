using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Network;

public interface IAdapter : IIdentifiable<string>
{
    ISocket Socket { get; }

    Task OnPacket(IPacket packet);
    Task OnException(Exception exception);
    Task OnDisconnect();

    Task Dispatch(IPacket packet);
    Task Disconnect();
}
