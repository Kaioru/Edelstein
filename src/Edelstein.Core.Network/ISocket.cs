using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public interface ISocket
    {
        uint SeqSend { get; set; }
        uint SeqRecv { get; set; }
        bool EncryptData { get; }

        Task SendPacket(IPacket packet);
        Task Close();
    }
}