using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields
{
    public class FieldObjPool : IFieldPool
    {
        private int _runningObjectID = 1;
        private readonly IList<IFieldObj> _objects;

        public FieldObjPool()
            => _objects = new List<IFieldObj>();

        public Task Enter(IFieldObj obj)
        {
            lock (this)
            {
                if (obj is IFieldUser user) obj.ID = user.Character.ID;
                else obj.ID = _runningObjectID++;

                _objects.Add(obj);
                return Task.CompletedTask;
            }
        }

        public Task Leave(IFieldObj obj)
        {
            lock (this)
            {
                _objects.Remove(obj);
                return Task.CompletedTask;
            }
        }

        public IFieldObj GetObject(int id)
            => _objects[id];

        public T GetObject<T>(int id) where T : IFieldObj
            => (T) _objects[id];

        public IEnumerable<IFieldObj> GetObjects()
            => _objects.ToImmutableList();

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => _objects.OfType<T>().ToImmutableList();

        public IFieldObj GetControlledObject(IFieldUser controller, int id)
            => GetControlledObjects(controller).First(o => o.ID == id);

        public T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlledObj
            => GetControlledObjects<T>(controller).First(o => o.ID == id);

        public IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller)
            => GetControlledObjects<IFieldControlledObj>(controller);

        public IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlledObj
            => _objects.OfType<T>().Where(o => o.Controller == controller).ToImmutableList();
    }
}