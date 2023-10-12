using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage4SkillHandler : Bmage3SkillHandler
{
    public override int ID => Job.Bmage4;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageNemesis:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageCyclone:
                context.AddTemporaryStat(TemporaryStatType.Cyclone, 1);
                break;
            case Skill.BmageStance:
                context.AddTemporaryStat(TemporaryStatType.Stance, context.SkillLevel!.Prop);
                break;
            case Skill.BmageShelter:
                // TODO
                context.AddAffectedArea(AffectedAreaType.Smoke);
                break;
            case Skill.BmageMapleHero:
                context.TargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.BmageHerosWill:
                context.ResetTemporaryStatNegative();
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
