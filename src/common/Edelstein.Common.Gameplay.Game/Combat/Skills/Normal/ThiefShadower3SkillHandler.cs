using Edelstein.Common.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Normal;

public class ThiefShadower3SkillHandler : ThiefShadower2SkillHandler
{
    public override int ID => Job.Thiefmaster;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.ThiefmasterShadowPartner:
                context.AddTemporaryStat(TemporaryStatType.ShadowPartner, 1);
                break;
            case Skill.ThiefmasterShadowMirror:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.AttackCounter);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
