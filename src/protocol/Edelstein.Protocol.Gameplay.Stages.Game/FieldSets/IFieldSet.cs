using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.FieldSets
{
    public interface IFieldSet : IFieldPool, IStage<IField, IFieldObjUser>, IRepositoryEntry<string>
    {
        IField GetField(int index);
        IEnumerable<IField> GetFields();

        Task OnUserEnter(IFieldObjUser user);
        Task OnUserLeave(IFieldObjUser user);

        Task OnUserMigrateField(IFieldObjUser user);
    }
}
