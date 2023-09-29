using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportConnectorState : ITransportContext
{
    private readonly IChannel _channel;
    private readonly IEventLoopGroup _group0;

    public NettyTransportConnectorState(IChannel channel, IEventLoopGroup group0, TransportVersion version, IReadOnlyRepository<string, ISocket> sockets)
    {
        _channel = channel;
        _group0 = group0;
        Version = version;
        Sockets = sockets;
    }

    public TransportState State => _channel.Active ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version { get; }

    public IReadOnlyRepository<string, ISocket> Sockets { get; }

    public async Task Dispatch(IPacket packet)
        => await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Dispatch(packet)));

    public async Task Close()
    {
        await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Close()));
        await _channel.CloseAsync();
        
#if (DEBUG)
        await _group0.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
#else
        await _group0.ShutdownGracefullyAsync(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));
#endif
    }
}
