using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game;

public class GameStage : AbstractStage<IGameStageUser>, IGameStage
{
    private readonly IFieldManager _fieldManager;

    public GameStage(string id, IFieldManager fieldManager)
    {
        ID = id;
        _fieldManager = fieldManager;
    }

    public override string ID { get; }

    public new async Task Enter(IGameStageUser user)
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

        user.FieldUser = fieldUser;

        await field.Enter(fieldUser);
        await fieldUser.ModifySkills(s => s.Set(Skill.KnightRestoration, 5));
        await fieldUser.ModifySkills(s => s.Set(Skill.KnightCombatOrders, 20));
        await fieldUser.ModifySkills(s => s.Set(Skill.PaladinBlast, 30, 30));
        await base.Enter(user);
    }

    public new async Task Leave(IGameStageUser user)
    {
        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
        await base.Leave(user);
    }
}
