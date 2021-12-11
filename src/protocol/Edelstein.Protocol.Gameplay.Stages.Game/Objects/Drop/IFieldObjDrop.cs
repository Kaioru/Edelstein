using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Drop
{
    public interface IFieldObjDrop : IFieldObj
    {
        DropOwnerType OwnerType { get; }

        int OwnerID { get; }
        int SourceID { get; }

        bool IsMoney { get; }
        int Info { get; }

        Task PickUp(IFieldObjUser user);
    }
}
