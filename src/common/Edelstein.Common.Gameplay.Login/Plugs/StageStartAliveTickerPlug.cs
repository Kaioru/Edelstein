using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class StageStartAliveTickerPlug : AbstractStageStartAliveTickerPlug<ILoginStageUser>
{
    public StageStartAliveTickerPlug(ITickerManager ticker, IStage<ILoginStageUser> stage) : base(ticker, stage)
    {
    }
}
