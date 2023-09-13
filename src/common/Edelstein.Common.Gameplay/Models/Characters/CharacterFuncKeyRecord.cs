using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterFuncKeyRecord : ICharacterFuncKeyRecord
{
    public byte Type { get; set; }
    public int Action { get; set; }
}
