using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.FieldSets
{
    public interface IFieldSetRepository : ILocalRepositoryReader<string, IFieldSet>
    {
        Task<IFieldSet> RetrieveByField(IField field);
    }
}
