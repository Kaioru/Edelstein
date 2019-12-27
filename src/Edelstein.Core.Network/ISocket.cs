using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public interface ISocket
    {
        uint SeqSend { get; set; }
        uint SeqRecv { get; set; }
        long ClientKey { get; set; }
        bool EncryptData { get; }

        Task SendPacket(IPacket packet);
        Task SendPacket(IEnumerable<IPacket> packets);
        Task Close();
    }
}