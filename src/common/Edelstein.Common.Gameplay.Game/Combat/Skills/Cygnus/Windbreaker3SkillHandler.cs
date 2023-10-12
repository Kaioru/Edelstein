using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat.Skills.Cygnus;

public class Windbreaker3SkillHandler : Windbreaker2SkillHandler
{
    public override int ID => Job.Windbreaker3;
    
    public override Task HandleSkillUse(ISkillContext context, IFieldUser user)
    {
        switch (context.Skill?.ID)
        {
            case Skill.WindbreakerPuppet:
                context.AddSummoned(MoveAbilityType.Stop, SummonedAssistType.None);
                break;
            case Skill.WindbreakerAlbatross:
                context.AddTemporaryStat(TemporaryStatType.Morph, context.SkillLevel!.Morph);
                break;
        }

        return base.HandleSkillUse(context, user);
    }
}
