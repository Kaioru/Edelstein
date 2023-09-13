namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterQuickslotKeys
{
    int? this[int key] { get; }
    IDictionary<int, int> Records { get; }
}
