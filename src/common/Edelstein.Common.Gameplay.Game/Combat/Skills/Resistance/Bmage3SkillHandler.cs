using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage3SkillHandler : Bmage2SkillHandler
{
    public override int ID => Job.Bmage3;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageConversion:
                context.AddTemporaryStat(TemporaryStatType.Conversion, context.SkillLevel!.X);
                break;
            case Skill.BmageSuperBody:
            case Skill.BmageSuperBodyDark:
            case Skill.BmageSuperBodyBlue:
            case Skill.BmageSuperBodyYellow:
                // TODO
                break;
            case Skill.BmageRevive:
                // TODO
                context.AddTemporaryStat(TemporaryStatType.Revive, context.SkillLevel!.X);
                break;
            case Skill.BmageTeleportMastery:
                context.AddTemporaryStat(TemporaryStatType.TeleportMasteryOn, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
