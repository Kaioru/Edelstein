using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public class CharacterExtendSP : ICharacterExtendSP
{
    public byte? this[byte jobLevel] => Records.TryGetValue(jobLevel, out var record) ? record : null;
    public IDictionary<byte, byte> Records { get; } = new Dictionary<byte, byte>();
}
