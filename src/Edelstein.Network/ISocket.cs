using System;
using System.Threading.Tasks;
using Edelstein.Network.Packet;

namespace Edelstein.Network
{
    public interface ISocket
    {
        uint SeqSend { get; set; }
        uint SeqRecv { get; set; }
        bool EncryptData { get; }
        
        bool ReadOnlyMode { get; set; }

        object LockSend { get; }
        object LockRecv { get; }

        Task OnPacket(IPacket packet);
        Task OnDisconnect();
        Task OnUpdate();
        Task OnException(Exception exception);

        Task Disconnect();
        Task SendPacket(IPacket packet);
    }
}