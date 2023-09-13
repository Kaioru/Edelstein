using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterFuncKeys : ICharacterFuncKeys
{
    public ICharacterFuncKeyRecord? this[int key] => Records.TryGetValue(key, out var record) ? record : null;
    public IDictionary<int, ICharacterFuncKeyRecord> Records { get; } = new Dictionary<int, ICharacterFuncKeyRecord>();
}
