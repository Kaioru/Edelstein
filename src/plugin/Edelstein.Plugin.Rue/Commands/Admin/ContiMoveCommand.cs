using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class ContiMoveCommand : AbstractCommand
{
    private readonly IContiMoveManager _manager;

    public override string Name => "ContiMove";
    public override string Description => "Shows the contimove schedule";

    public ContiMoveCommand(IContiMoveManager manager)
    {
        _manager = manager;
    }

    public override async Task Execute(IFieldUser user, string[] args)
    {
        var contiMoves = await _manager.RetrieveAll();
        var contiMoveID = await user.Prompt(target => target.AskMenu(
            "Here are the ship schedules",
            contiMoves.ToDictionary(
                c => c.ID,
                c =>
                {
                    var ret = $"{c.Template.Name} ({c.State})";

                    if (c.State == ContiMoveState.Event)
                        ret += " #r(Event ongoing)#b";

                    return ret;
                }
            )
        ), -1);
        var contiMove = await _manager.Retrieve(contiMoveID);

        if (contiMove != null)
            await contiMove.Enter(user);
    }
}
