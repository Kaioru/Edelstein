using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportAcceptorState(
    IChannel channel, 
    IEventLoopGroup group0, 
    IEventLoopGroup group1, 
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
        var t0 = group0.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
        var t1 = group1.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);

        await Task.WhenAll(t0, t1);
#else
        var t0 = group0.ShutdownGracefullyAsync(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        var t1 = group1.ShutdownGracefullyAsync(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

        await Task.WhenAll(t0, t1);
#endif
    }
}
