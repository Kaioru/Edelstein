using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public interface ICharacterRepository
        : IRepositoryReader<int, CharacterEntity>
        , IRepositoryWriter<int, CharacterEntity>
    {
    }
}
