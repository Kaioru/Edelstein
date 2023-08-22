using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game;

public class GameStage : AbstractStage<IGameStageUser>, IGameStage
{
    private readonly IFieldManager _fieldManager;
    
    public override string ID { get; }
    
    public GameStage(string id, IFieldManager fieldManager)
    {
        ID = id;
        _fieldManager = fieldManager;
    }
    
    public async Task Enter(IGameStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }

        var field = await _fieldManager.Retrieve(user.Character.FieldID);
        var fieldUser = new FieldUser(user, user.Account, user.AccountWorld, user.Character);

        if (field == null)
        {
            await user.Disconnect();
            return;
        }

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

}
