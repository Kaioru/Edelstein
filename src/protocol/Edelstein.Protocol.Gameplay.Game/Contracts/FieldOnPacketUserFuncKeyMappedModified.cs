using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserFuncKeyMappedModified(
    IFieldUser User,
    IDictionary<int, ICharacterFuncKeyRecord> Keys
);
