using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportAcceptorState : ITransportContext
{
    private readonly IChannel _channel;
    private readonly IEventLoopGroup _group0;
    private readonly IEventLoopGroup _group1;

    public TransportState State => _channel.Active ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version { get; }
    
    public IReadOnlyRepository<string, ISocket> Sockets { get; }
    
    public NettyTransportAcceptorState(IChannel channel, IEventLoopGroup group0, IEventLoopGroup group1, TransportVersion version, IReadOnlyRepository<string, ISocket> sockets)
    {
        _channel = channel;
        _group0 = group0;
        _group1 = group1;
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
        await _group1.ShutdownGracefullyAsync();
    }
}
