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
    }
}