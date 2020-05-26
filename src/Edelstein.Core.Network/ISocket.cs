using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Network
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