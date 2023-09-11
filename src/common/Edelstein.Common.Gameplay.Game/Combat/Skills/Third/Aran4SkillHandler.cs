using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Aran4SkillHandler : Aran3SkillHandler
{
    public override int ID => Job.Aran4;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AranMapleHero:
                context.SetTargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.AranFreezeStanding:
                context.AddTemporaryStat(TemporaryStatType.Stance, context.SkillLevel!.Prop);
                break;
            case Skill.AranComboBarrier:
                context.AddTemporaryStat(TemporaryStatType.ComboBarrier, context.SkillLevel!.X);
                break;
            case Skill.AranHerosWill:
                context.ResetTemporaryStatNegative();
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
