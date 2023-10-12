using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Windbreaker2SkillHandler : Windbreaker1SkillHandler
{
    public override int ID => Job.Windbreaker2;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WindbreakerBowBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.WindbreakerSoulArrowBow:
                context.AddTemporaryStat(TemporaryStatType.SoulArrow, 1);
                break;
            case Skill.WindbreakerWindWalk:
                context.AddTemporaryStat(TemporaryStatType.DarkSight, context.SkillLevel!.X);
                context.AddTemporaryStat(TemporaryStatType.Speed, -context.SkillLevel!.Y);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
