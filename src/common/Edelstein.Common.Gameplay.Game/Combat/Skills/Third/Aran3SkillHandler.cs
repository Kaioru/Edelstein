using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Third;

public class Aran3SkillHandler : Aran2SkillHandler
{
    public override int ID => Job.Aran3;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        // TODO snow charge
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.AranSnowCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
            case Skill.AranSmartKnockback:
                context.AddTemporaryStat(TemporaryStatType.SmartKnockback, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
