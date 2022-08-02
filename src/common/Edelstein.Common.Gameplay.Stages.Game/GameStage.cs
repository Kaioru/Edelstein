using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStage : IGameStage
{
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
}
