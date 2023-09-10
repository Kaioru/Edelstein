using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat;

public record SkillContextTargetField(
    int Limit,
    IRectangle2D Bounds
);
