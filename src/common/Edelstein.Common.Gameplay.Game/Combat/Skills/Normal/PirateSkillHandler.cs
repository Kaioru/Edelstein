using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PirateSkillHandler : NoviceSkillHandler
{
    public override int ID => Job.Pirate;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PirateDash:
                context.SetTwoStateDashSpeed(context.SkillLevel!.X);
                context.SetTwoStateDashJump(context.SkillLevel!.Y);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
