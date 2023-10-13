using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class MagicianThunderCold2SkillHandler : Magician1SkillHandler
{
    public override int ID => Job.WizardThunderCold;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Wizard2ColdBeam:
                context.AddMobTemporaryStat(MobTemporaryStatType.Freeze, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Wizard2Meditation:
                context.TargetParty();
                break;
            case Skill.Wizard2Slow:
                context.TargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.Speed, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
