using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Trade.Handling.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<ITradeStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, ITradeStage stage) : base(ticker, stage)
    {
    }
}
