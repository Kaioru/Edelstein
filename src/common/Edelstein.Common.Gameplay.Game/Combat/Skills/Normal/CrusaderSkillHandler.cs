using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class CrusaderSkillHandler : FighterSkillHandler
{
    public override int ID => Job.Crusader;

    public override async Task HandleAttack(ISkillContext context, IFieldUser user)
    {
        await this.HandleAttackComboCounter(context, user);
        await base.HandleAttack(context, user);
    }

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CrusaderPanic:
                if (context.Random.Next(0, 100) <= context.SkillLevel!.Prop)
                    context.AddMobTemporaryStat(MobTemporaryStatType.ACC, -context.SkillLevel!.X);
                break;
            case Skill.CrusaderComa:
            case Skill.CrusaderShout:
                if (context.Random.Next(0, 100) <= context.SkillLevel!.Prop)
                    context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
        }
        
        return base.HandleAttackMob(context, user, mob);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.CrusaderComboAttack:
                context.AddTemporaryStat(TemporaryStatType.ComboCounter, 1);
                break;
            case Skill.CrusaderMagicCrash:
                // TODO: remove existing buffs
                context.SetTargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.MagicCrash, 1);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
