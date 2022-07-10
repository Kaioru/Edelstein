using System.Threading.Tasks;

namespace Edelstein.Protocol.Network.Transport
{
    public interface ISocket : IPacketDispatcher
    {
        string ID { get; }

        uint SeqSend { get; set; }
        uint SeqRecv { get; set; }
        bool EncryptData { get; init; }

        Task Disconnect();
    }
}
