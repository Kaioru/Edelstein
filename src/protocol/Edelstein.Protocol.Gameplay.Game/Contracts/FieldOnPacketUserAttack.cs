using Edelstein.Protocol.Gameplay.Game.Combat.Damage.Requests;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserAttack(
    IFieldUser User,
    IAttackRequest Attack
);
