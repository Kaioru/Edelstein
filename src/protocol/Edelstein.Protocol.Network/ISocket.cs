using System.Net;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Repositories;

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
