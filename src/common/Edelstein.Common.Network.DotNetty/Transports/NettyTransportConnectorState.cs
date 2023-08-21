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

    public TransportState State => _channel.Active ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version { get; }
    
    public IReadOnlyRepository<string, ISocket> Sockets { get; }
    
    public NettyTransportConnectorState(IChannel channel, IEventLoopGroup group0, TransportVersion version, IReadOnlyRepository<string, ISocket> sockets)
    {
        _channel = channel;
        _group0 = group0;
        Version = version;
        Sockets = sockets;
    }

    public async Task Dispatch(IPacket packet) 
        => await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Dispatch(packet)));

    public async Task Close()
    {
        await Task.WhenAll((await Sockets.RetrieveAll()).Select(s => s.Close()));
        await _channel.CloseAsync();
        await _group0.ShutdownGracefullyAsync();
    }
}
