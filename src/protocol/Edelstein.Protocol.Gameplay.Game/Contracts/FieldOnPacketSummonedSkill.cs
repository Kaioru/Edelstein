using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketSummonedSkill(
    IFieldUser User,
    IFieldSummoned Summoned,
    int SkillID,
    IFieldSummonedMoveAction MoveAction,
    byte AttackAction
);
