using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterQuickslotKeys : ICharacterQuickslotKeys
{
    public int? this[int key] => Records.TryGetValue(key, out var record) ? record : null;
    public IDictionary<int, int> Records { get; } = new Dictionary<int, int>();
}
