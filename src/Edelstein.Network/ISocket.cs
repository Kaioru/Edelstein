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

        Task OnPacket(IPacket packet);
        Task OnException(Exception exception);
        Task OnUpdate();
        Task OnDisconnect();

        Task SendPacket(IPacket packet);
        Task Close();
    }
}