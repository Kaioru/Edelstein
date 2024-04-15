using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Protocol.Network;

public interface ISocketUser
{
    ISocket Socket { get; }

    Task Dispatch(IPacket packet) => Socket.Dispatch(packet);
    Task Disconnect() => Socket.Close();
}
