using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Trade;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Trade.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<ITradeStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, ITradeStage stage) : base(ticker, stage)
    {
    }
}
