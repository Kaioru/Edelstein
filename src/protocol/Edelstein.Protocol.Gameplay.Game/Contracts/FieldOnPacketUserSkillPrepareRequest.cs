using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserSkillPrepareRequest(
    IFieldUser User,
    int SkillID,
    byte SkillLevel,
    short MoveAction,
    byte ActionSpeed
);
