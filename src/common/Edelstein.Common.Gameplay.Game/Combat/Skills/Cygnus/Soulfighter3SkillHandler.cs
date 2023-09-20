using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Soulfighter3SkillHandler: Soulfighter2SkillHandler
{
    public override int ID => Job.Soulfighter3;

    public override async Task HandleAttack(ISkillContext context, IFieldUser user)
    {
        await this.HandleAttackComboCounter(context, user);
        await base.HandleAttack(context, user);
    }

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.SoulmasterComboAttack:
                context.AddTemporaryStat(TemporaryStatType.ComboCounter, 1);
                break;
            case Skill.SoulmasterPanicSword:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.ACC, -context.SkillLevel!.X);
                break;
            case Skill.SoulmasterComaSword:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Stun, 1);
                break;
            case Skill.SoulmasterSoulCharge:
                context.AddTemporaryStat(TemporaryStatType.WeaponCharge, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
