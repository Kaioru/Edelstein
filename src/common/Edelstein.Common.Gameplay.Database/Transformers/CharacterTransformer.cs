using Edelstein.Protocol.Gameplay.Characters;

namespace Edelstein.Common.Gameplay.Database.Transformers;

public class CharacterTransformer : ITransformer<ICharacter>
{
    public ICharacter Transform(ITransformerContext ctx) => throw new NotImplementedException();
    public ITransformerContext Transform(ICharacter obj) => throw new NotImplementedException();
}
