using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextSummoned(
    MoveAbilityType MoveAbilityType, 
    SummonedAssistType SummonedAssistType, 
    int SkillID, 
    int SkillLevel, 
    DateTime Expire
);
