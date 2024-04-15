using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportConnectorState(
    IChannel channel, 
    IEventLoopGroup group0, 
    TransportVersion version, 
    IReadOnlyRepository<string, ISocket> sockets
)
    : ITransportContext
{

    public TransportState State => channel.Active ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version { get; } = version;

    public IReadOnlyRepository<string, ISocket> Sockets { get; } = sockets;

    public async Task Dispatch(IPacket packet)
        => await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Dispatch(packet)));

    public async Task Close()
    {
        await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Close()));
        await channel.CloseAsync();
        
#if (DEBUG)
        await group0.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
#else
        await _group0.ShutdownGracefullyAsync(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));
#endif
    }
}
