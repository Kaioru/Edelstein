using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class GameStage : IGameStage
{
    private readonly IFieldManager _fieldManager;

    public GameStage(IFieldManager fieldManager) => _fieldManager = fieldManager;

    public IReadOnlyCollection<IGameStageUser> Users => new List<IGameStageUser>();

    public async Task Enter(IGameStageUser user)
    {
        if (user.Character == null) return;

        var field = await _fieldManager.Retrieve(user.Character.FieldID);
        var fieldUser = field?.CreateUser(user);

        if (field == null || fieldUser == null) return;

        user.Stage = this;
        user.FieldUser = fieldUser;
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
