using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Protocol.Network;

public interface ISocket : ITransportContext
{
    uint SeqSend { get; set; }
    uint SeqRecv { get; set; }

    bool IsDataEncrypted { get; }
}
