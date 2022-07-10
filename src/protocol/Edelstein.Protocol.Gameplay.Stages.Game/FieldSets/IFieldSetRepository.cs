using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.FieldSets
{
    public interface IFieldSetRepository : ILocalRepository<string, IFieldSet>
    {
        Task<IFieldSet> RetrieveByField(IField field);
    }
}
