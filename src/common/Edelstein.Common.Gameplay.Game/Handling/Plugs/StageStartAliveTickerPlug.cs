using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<IGameStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, IGameStage stage) : base(ticker, stage)
    {
    }
}
