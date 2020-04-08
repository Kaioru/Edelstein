using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public class FieldPool : IFieldPool
    {
        private readonly IDictionary<int, IFieldObj> _objects;
        private int _runningObjectID = 1;

        public FieldPool()
            => _objects = new ConcurrentDictionary<int, IFieldObj>();

        public Task Enter(IFieldObj obj)
        {
            lock (this)
            {
                if (obj is IFieldUser user) obj.ID = user.Character.ID;
                else obj.ID = _runningObjectID++;

                if (_runningObjectID > int.MaxValue - 1)
                    _runningObjectID = 1;

                _objects[obj.ID] = obj;
                return Task.CompletedTask;
            }
        }

        public Task Leave(IFieldObj obj)
        {
            lock (this)
            {
                _objects.Remove(obj.ID);
                return Task.CompletedTask;
            }
        }

        public IFieldObj GetObject(int id)
            => _objects[id];

        public T GetObject<T>(int id) where T : IFieldObj
            => (T) _objects[id];

        public IEnumerable<IFieldObj> GetObjects()
            => _objects.Values.ToImmutableList();

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => _objects.Values.OfType<T>().ToImmutableList();

        public IFieldObj GetControlledObject(IFieldUser controller, int id)
            => GetControlledObjects(controller).FirstOrDefault(o => o.ID == id);

        public T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlled
            => GetControlledObjects<T>(controller).FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller)
            => GetControlledObjects<IFieldControlled>(controller);

        public IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlled
            => GetObjects<T>().Where(o => o.Controller == controller).ToImmutableList();
    }
}