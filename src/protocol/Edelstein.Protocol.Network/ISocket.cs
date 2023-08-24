using System.Net;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Network;

public interface ISocket : IIdentifiable<string>
{
    EndPoint AddressLocal { get; }
    EndPoint AddressRemote { get; }

    uint SeqSend { get; set; }
    uint SeqRecv { get; set; }

    bool IsDataEncrypted { get; }

    DateTime LastAliveSent { get; set; }
    DateTime LastAliveRecv { get; set; }

    Task Dispatch(IPacket packet);
    Task Close();
}
