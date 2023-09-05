using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<IShopStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, IShopStage stage) : base(ticker, stage)
    {
    }
}
