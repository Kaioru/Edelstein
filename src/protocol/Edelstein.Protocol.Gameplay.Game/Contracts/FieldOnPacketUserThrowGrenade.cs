using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserThrowGrenade(
    IFieldUser User,
    IPoint2D Position,
    int Y2,
    int Keydown,
    int SkillID,
    int SkillLevel
);
