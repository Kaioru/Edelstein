using System.Net;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network;

public interface ISocket
{
    string ID { get; }
    
    IPEndPoint AddressLocal { get; }
    IPEndPoint AddressRemote { get; }
    
    uint SeqSend { get; set; }
    uint SeqRecv { get; set; }
    
    bool IsDataEncrypted { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
