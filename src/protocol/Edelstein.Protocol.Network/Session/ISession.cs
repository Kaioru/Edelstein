using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Protocol.Network.Session
{
    public interface ISession : IPacketDispatcher
    {
        string ID { get; }

        ISocket Socket { get; init; }

        Task OnPacket(IPacketReader packet);
        Task OnException(Exception exception);
        Task OnDisconnect();

        Task Update();
        Task Disconnect();
    }
}
