using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldPool
    {
        Task Enter(IFieldObj obj);
        Task Leave(IFieldObj obj);

        IFieldObj GetObject(int id);
        T GetObject<T>(int id) where T : IFieldObj;

        IEnumerable<IFieldObj> GetObjects();
        IEnumerable<T> GetObjects<T>() where T : IFieldObj;
        
        
        IFieldObj GetControlledObject(IFieldUser controller, int id);
        T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlledObj;

        IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller);
        IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlledObj;
    }
}