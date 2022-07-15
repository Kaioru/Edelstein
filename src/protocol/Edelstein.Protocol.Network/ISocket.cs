using System.Net;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Network;

public interface ISocket : IIdentifiable<string>
{
    EndPoint AddressLocal { get; }
    EndPoint AddressRemote { get; }

    uint SeqSend { get; set; }
    uint SeqRecv { get; set; }

    bool IsDataEncrypted { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
