using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IFieldPool : IFieldDispatcher
    {
        Task Enter(IFieldObj obj);
        Task Leave(IFieldObj obj);

        IFieldObj GetObject(int id);
        T GetObject<T>(int id) where T : IFieldObj;

        IEnumerable<IFieldObj> GetObjects();
        IEnumerable<T> GetObjects<T>() where T : IFieldObj;
    }
}
