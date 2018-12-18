using System;
using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public interface ISocket
    {
        uint SeqSend { get; set; }
        uint SeqRecv { get; set; }
        bool EncryptData { get; }

        object LockSend { get; }
        object LockRecv { get; }

        Task OnPacket(IPacket packet);
        Task OnDisconnect();
        Task OnException(Exception exception);

        Task Disconnect();
        Task SendPacket(IPacket packet);
    }
}