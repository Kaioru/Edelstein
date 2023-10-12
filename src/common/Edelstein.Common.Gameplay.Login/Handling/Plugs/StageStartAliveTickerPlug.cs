using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<ILoginStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, ILoginStage stage) : base(ticker, stage)
    {
    }
}
