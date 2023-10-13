using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Evan6SkillHandler : Evan5SkillHandler
{
    public override int ID => Job.Evan6;

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        // TODO EvanSlow
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.EvanMagicBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.EvanSlow:
                context.AddTemporaryStat(TemporaryStatType.EvanSlow, 1);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
