using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefNightlord3SkillHandler : ThiefNightlord2SkillHandle
{
    public override int ID => Job.Hermit;

    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.HermitShadowPartner:
                context.AddTemporaryStat(TemporaryStatType.ShadowPartner, 1);
                break;
            case Skill.HermitShadowMirror:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.AttackCounter);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
