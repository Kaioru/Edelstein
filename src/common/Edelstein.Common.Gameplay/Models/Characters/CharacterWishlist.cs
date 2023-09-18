using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public record CharacterWishlist : ICharacterWishlist
{
    public IList<int> Records { get; private set; } = new List<int>();
}
