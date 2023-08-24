using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<IGameStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, IGameStage stage) : base(ticker, stage)
    {
    }
}
