using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Resistance;

public class Bmage2SkillHandler : Bmage1SkillHandler
{
    public override int ID => Job.Bmage2;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.BmageAuraBlue:
                // TODO advanced
                context.ResetTemporaryStatAuras();
                context.AddTemporaryStat(TemporaryStatType.BlueAura, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
            case Skill.BmageAuraYellow:
                // TODO advanced
                context.ResetTemporaryStatAuras();
                context.AddTemporaryStat(TemporaryStatType.YellowAura, context.SkillLevel!.X, expire: DateTime.MaxValue);
                break;
            case Skill.BmageBloodDrain:
                // TODO
                break;
            case Skill.BmageStaffBooster:
                context.AddTemporaryStat(TemporaryStatType.Booster, context.SkillLevel!.X);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
