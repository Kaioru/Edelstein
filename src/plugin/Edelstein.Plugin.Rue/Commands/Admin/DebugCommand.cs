using Edelstein.Common.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class DebugCommand : AbstractCommand
{
    public override string Name => "Debug";
    public override string Description => "Testing command for debugging purposes";
    
    public override async Task Execute(IFieldUser user, string[] args)
    {
        var bounds = new Rectangle2D(
            new Point2D(user.Position.X - 200, user.Position.Y - 150),
            new Point2D(user.Position.X + 200, user.Position.Y + 150)
        );
        if (user.Field != null)
            await user.Field.Enter(new FieldAffectedArea(
                user.Character.ID,
                AffectedAreaType.UserSkill,
                2111003, 
                20, 
                0,
                0, 
                bounds
            ));
    }
}
