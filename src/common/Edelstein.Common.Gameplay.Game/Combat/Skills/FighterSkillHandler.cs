using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills;

public class FighterSkillHandler : SwordmanSkillHandler
{
    public override int ID => Job.Fighter;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.FighterWeaponBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
            case Skill.FighterFury:
                context.SetTargetParty();
                break;
            case Skill.FighterPowerGuard:
                context.AddTemporaryStat(TemporaryStatType.PowerGuard, context.SkillLevel!.X);
                break;
        }
        
        return base.HandleSkillUse(context, user);
    }
}
