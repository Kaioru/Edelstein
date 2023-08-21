using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransportContext
{
    TransportState State { get; }
    TransportVersion Version { get; }
    
    IReadOnlyRepository<string, ISocket> Sockets { get; }

    Task Dispatch(IPacket packet);
    Task Close();
}
