using System;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Network;

public interface ISocket : IRepositoryEntry<string>
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
