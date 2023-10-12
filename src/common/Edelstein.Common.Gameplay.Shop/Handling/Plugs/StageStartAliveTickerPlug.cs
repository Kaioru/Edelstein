using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<IShopStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, IShopStage stage) : base(ticker, stage)
    {
    }
}
