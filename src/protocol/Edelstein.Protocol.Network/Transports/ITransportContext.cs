using System.Net;
using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Network.Transports;

public interface ITransportContext
{
    string ID { get; }

    TransportState State { get; }
    TransportVersion Version { get; }
    
    EndPoint AddressLocal { get; }
    EndPoint AddressRemote { get; }
    
    Task Dispatch(IDispatchable dispatch);
    Task Close();
}
