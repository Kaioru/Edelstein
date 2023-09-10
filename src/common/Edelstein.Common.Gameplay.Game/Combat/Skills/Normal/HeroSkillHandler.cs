using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class HeroSkillHandler : CrusaderSkillHandler
{
    public override int ID => Job.Hero;

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.HeroMonsterMagnet:
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.HeroMapleHero:
                context.SetTargetParty();
                context.AddTemporaryStat(TemporaryStatType.BasicStatUp, context.SkillLevel!.X);
                break;
            case Skill.HeroStance:
                context.AddTemporaryStat(TemporaryStatType.Stance, context.SkillLevel!.Prop);
                break;
            case Skill.HeroEnrage:
                context.ResetComboCounter();
                context.AddTemporaryStat(TemporaryStatType.Enrage, (short)(
                    context.SkillLevel!.X * 100 +
                    context.SkillLevel!.MobCount)
                );
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
