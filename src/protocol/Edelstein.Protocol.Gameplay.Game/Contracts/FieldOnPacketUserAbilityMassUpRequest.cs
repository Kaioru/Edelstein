using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserAbilityMassUpRequest(
    IFieldUser User,
    IDictionary<ModifyStatType, int> StatUp
);
