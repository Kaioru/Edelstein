using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class MagicianThunderCold3SkillHandler : MagicianThunderCold2SkillHandler
{
    public override int ID => Job.MageThunderCold;
    
    public override Task HandleAttackMob(ISkillContext context, IFieldUser user, IFieldMob mob)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Mage2IceStrike:
                context.AddMobTemporaryStat(MobTemporaryStatType.Freeze, 1);
                break;
            case Skill.Mage2ThunderSpear:
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
            case Skill.Mage2Seal:
                context.SetProc();
                context.AddMobTemporaryStat(MobTemporaryStatType.Seal, 1);
                break;
            case Skill.Mage2MagicBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.Mage2TeleportMastery:
                context.AddTemporaryStat(TemporaryStatType.TeleportMasteryOn, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
            case Skill.Mage2ElementalReset:
                context.AddTemporaryStat(TemporaryStatType.ElementalReset, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
