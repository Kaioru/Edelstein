using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class WarriorPaladin3SkillHandler : WarriorPaladin2SkillHandler
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
            case Skill.KnightIceCharge:
            case Skill.KnightLightningCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
            case Skill.KnightCombatOrders:
                context.AddTemporaryStat(TemporaryStatType.CombatOrders, context.SkillLevel!.X);
                break;
            case Skill.KnightMagicCrash:
                context.TargetField();
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.MagicCrash, 1);
                context.ResetMobTemporaryStatPositive();
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
