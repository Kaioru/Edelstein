using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserAbilityUpRequest(
    IFieldUser User,
    ModifyStatType Type
);
