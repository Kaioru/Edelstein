namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterExtendSP
{
    byte? this[byte jobLevel] { get; }
    IDictionary<byte, byte> Records { get; }
}
