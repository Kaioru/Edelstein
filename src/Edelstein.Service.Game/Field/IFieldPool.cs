using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Field
{
    public interface IFieldPool
    {
        Task Enter(IFieldObj obj);
        Task Leave(IFieldObj obj);

        IFieldObj GetObject(int id);
        T GetObject<T>(int id) where T : IFieldObj;

        IEnumerable<IFieldObj> GetObjects();
        IEnumerable<T> GetObjects<T>() where T : IFieldObj;
    }
}