using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStage : IGameStage, ITickable
{
    public IReadOnlyCollection<IGameStageUser> Users => new List<IGameStageUser>();

    public async Task Enter(IGameStageUser user)
    {
        var fieldUser = new FieldUser(
            user,
            user.Account!,
            user.AccountWorld!,
            user.Character!
        );

        await user.Dispatch(fieldUser.GetSetFieldPacket());
    }

    public Task Leave(IGameStageUser user) => Task.CompletedTask;

    public Task OnTick(DateTime now) => Task.CompletedTask;
}
