using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network;

public interface ISocketUserAdapter<in TSocketUser>
    where TSocketUser : ISocketUser
{
    Task OnPacket(TSocketUser user, IRawPacket packet);
    Task OnException(TSocketUser user, Exception exception);
    Task OnDisconnect(TSocketUser user);
}
