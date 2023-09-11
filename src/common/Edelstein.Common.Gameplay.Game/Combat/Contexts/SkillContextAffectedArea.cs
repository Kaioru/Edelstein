using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextAffectedArea(
    AffectedAreaType Type, 
    int SkillID, 
    int SkillLevel, 
    int Info, 
    int Phase, 
    IRectangle2D Bounds, 
    DateTime? Expire = null
);
