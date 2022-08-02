using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Daemon.Server.Bootstraps;

public class SocketAliveBootstrap : IBootstrap, ITickable
{
    private readonly ITransportAcceptor _acceptor;
    private readonly TimeSpan _frequency;
    private readonly TimeSpan _schedule;
    private readonly ITickerManager _ticker;

    public SocketAliveBootstrap(ITickerManager ticker, ITransportAcceptor acceptor)
    {
        _ticker = ticker;
        _acceptor = acceptor;
        _frequency = _acceptor.Timeout.Divide(2);
        _schedule = _acceptor.Timeout.Divide(4);
    }

    private ITickerManagerContext? Context { get; set; }

    public Task Start()
    {
        Context = _ticker.Schedule(this, _schedule);
        return Task.CompletedTask;
    }


    public Task Stop()
    {
        Context?.Cancel();
        return Task.CompletedTask;
    }

    public async Task OnTick(DateTime now)
    {
        foreach (var socket in _acceptor.Sockets.Values)
            if (now - socket.LastAliveSent > _frequency)
            {
                socket.LastAliveSent = now;
                await socket.Dispatch(new PacketWriter(PacketSendOperations.AliveReq));
            }
    }
}
