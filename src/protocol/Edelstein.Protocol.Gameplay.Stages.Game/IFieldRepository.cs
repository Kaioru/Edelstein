using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IFieldRepository
        : ILocalRepositoryReader<int, IField>
        , ILocalRepositoryWriter<int, IField>
    {
    }
}
