using System.Threading.Tasks;
using Cysharp.Threading;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Utilities.Tickers;

public class TickerPool(
    int ticksPerSecond,
    int poolSize
) : ITicker
{
    private readonly LogicLooperPool _looper = new(ticksPerSecond, poolSize, RoundRobinLogicLooperPoolBalancer.Instance);

    public async Task Register(ITickerAction action)
        => await _looper.RegisterActionAsync((in LogicLooperActionContext _) =>
        {
            action.Act();
            return true;
        });

    public void Dispose()
        => _looper.Dispose();
}
