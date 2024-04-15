using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Protocol.Network;

public interface ISocketAdapter<in TSocketUser>
    where TSocketUser : ISocketUser
{
    Task OnPacket(TSocketUser user, IPacket packet);
    Task OnException(TSocketUser user, Exception exception);
    Task OnDisconnect(TSocketUser user);
}
