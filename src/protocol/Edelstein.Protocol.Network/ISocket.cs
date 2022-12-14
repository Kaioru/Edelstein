using System.Net;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Util;
using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Network;

public interface ISocket : IIdentifiable<string>
{
    EndPoint AddressLocal { get; }
    EndPoint AddressRemote { get; }

    bool IsDataEncrypted { get; }
    uint SeqSend { get; set; }
    uint SeqRecv { get; set; }

    Task Dispatch(IPacket packet);
    Task Close();
}
