using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class KnightSkillHandler : PageSkillHandler
{
    public override int ID => Job.Knight;

    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.KnightIceCharge:
                context.AddMobTemporaryStat(MobTemporaryStatType.Freeze, 1);
                break;
            case Skill.KnightChargeBlow:
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
            case Skill.KnightIceCharge:
            case Skill.KnightLightningCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
            case Skill.KnightCombatOrders:
                context.AddTemporaryStat(TemporaryStatType.CombatOrders, context.SkillLevel!.X);
                break;
            case Skill.KnightMagicCrash:
                context.SetTargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.MagicCrash, 1);
                break;
            case Skill.KnightRestoration:
                // TODO
                break;
            case Skill.KnightFireCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
