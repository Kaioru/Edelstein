using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStage : IGameStage
{
    private readonly IFieldManager _fieldManager;

    public GameStage(IFieldManager fieldManager) => _fieldManager = fieldManager;

    public IReadOnlyCollection<IGameStageUser> Users => new List<IGameStageUser>();

    public async Task Enter(IGameStageUser user)
    {
        var fieldUser = new FieldUser(
            user,
            user.Account!,
            user.AccountWorld!,
            user.Character!
        );
        var field = await _fieldManager.Retrieve(fieldUser.Character.FieldID);

        user.Stage = this;
        user.FieldUser = fieldUser;

        if (field != null)
            await field.Enter(fieldUser);
    }

    public async Task Leave(IGameStageUser user)
    {
        user.Stage = null;

        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
    }

    public Task OnTick(DateTime now) => Task.CompletedTask;
}
