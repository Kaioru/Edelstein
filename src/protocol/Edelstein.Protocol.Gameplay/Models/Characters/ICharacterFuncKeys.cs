namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterFuncKeys
{
    ICharacterFuncKeyRecord? this[int key] { get; }
    IDictionary<int, ICharacterFuncKeyRecord> Records { get; }
}
