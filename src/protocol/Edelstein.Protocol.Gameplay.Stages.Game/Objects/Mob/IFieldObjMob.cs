using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob
{
    public interface IFieldObjMob : IFieldControlledObj, IFieldLife
    {
        IFieldObjMob Info { get; }

        int HP { get; }
        int MP { get; }

        Task Hit(IFieldObjUser user, int damage);
        Task Kill(IFieldObjUser user);
    }
}
