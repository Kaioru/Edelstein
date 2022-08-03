using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStageTicker : ITickable
{
    public Task OnTick(DateTime now) => Task.CompletedTask;
}
