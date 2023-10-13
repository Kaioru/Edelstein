using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefDual1SkillHandler : Thief1SkillHandler
{
    public override int ID => Job.Dual1;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Dual1DualBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
