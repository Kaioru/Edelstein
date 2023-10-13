using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefDual5SkillHandler : ThiefDual4SkillHandler
{
    public override int ID => Job.Dual5;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.Dual5ThornsEffect:
                var cr = context.SkillLevel!.X;
                var cd = context.SkillLevel!.CDMax;

                context.AddTemporaryStat(TemporaryStatType.ThornsEffect, (cr << 8) + cd);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
