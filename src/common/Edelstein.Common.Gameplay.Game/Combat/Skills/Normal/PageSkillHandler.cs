using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class PageSkillHandler : SwordmanSkillHandler
{
    public override int ID => Job.Page;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.PageWeaponBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.PageThreaten:
                context.TargetField();
                context.AddMobTemporaryStat(MobTemporaryStatType.PAD, context.SkillLevel!.X);
                context.AddMobTemporaryStat(MobTemporaryStatType.PDR, context.SkillLevel!.Y);
                context.AddMobTemporaryStat(MobTemporaryStatType.ACC, -context.SkillLevel!.Z);
                break;
            case Skill.PagePowerGuard:
                context.AddTemporaryStat(TemporaryStatType.PowerGuard, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
