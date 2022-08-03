using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Daemon.Server.Tickers;

public class AliveTicker : ITickable
{
    private readonly ITransportAcceptor _acceptor;
    private readonly TimeSpan _aliveFrequency;
    private readonly TimeSpan _aliveSchedule;

    public AliveTicker(ITransportAcceptor acceptor)
    {
        _acceptor = acceptor;
        _aliveFrequency = _acceptor.Timeout.Divide(2);
        _aliveSchedule = _acceptor.Timeout.Divide(4);
    }

    private DateTime AliveLast { get; set; }

    public async Task OnTick(DateTime now)
    {
        if (now - AliveLast > _aliveSchedule)
        {
            AliveLast = now;

            foreach (var socket in _acceptor.Sockets.Values)
                if (now - socket.LastAliveSent > _aliveFrequency)
                {
                    socket.LastAliveSent = now;
                    await socket.Dispatch(new PacketWriter(PacketSendOperations.AliveReq));
                }
        }
    }
}
