using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextSummoned(
    MoveAbilityType MoveAbilityType, 
    SummonedAssistType SummonedAssistType, 
    int SkillID, 
    int SkillLevel, 
    bool AllowDuplicate,
    DateTime Expire,
    IPoint2D Position
);
