using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Windbreaker4SkillHandler : Windbreaker3SkillHandler
{
    public override int ID => Job.Windbreaker4;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        return base.HandleSkillUse(context, user);
    }
}
