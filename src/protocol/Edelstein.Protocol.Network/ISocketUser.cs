using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network;

public interface ISocketUser
{
    ISocket Socket { get; }

    Task Dispatch(IDispatchable dispatch) => Socket.Dispatch(dispatch);
    Task Disconnect() => Socket.Close();
}
