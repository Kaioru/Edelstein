using System;
using System.Threading.Tasks;
using Cysharp.Threading;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Utilities.Tickers;

public class Ticker(
    int ticksPerSecond
) : ITicker
{
    private readonly LogicLooper _looper = new(ticksPerSecond);

    public async Task Register(ITickerAction action)
        => await _looper.RegisterActionAsync((in LogicLooperActionContext _) =>
        {
            action.Act();
            return true;
        });

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _looper.Dispose();
    }
}
