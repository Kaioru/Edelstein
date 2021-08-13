using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class FieldPool : IFieldPool
    {
        private int _runningObjectID;
        private readonly IDictionary<int, IFieldObj> _objects;
        private readonly object _lock;

        public FieldPool(int startObjectID = 1)
        {
            _runningObjectID = startObjectID;
            _objects = new Dictionary<int, IFieldObj>();
            _lock = new object();
        }

        public Task Enter(IFieldObj obj)
        {
            lock (_lock)
            {
                obj.ID = _runningObjectID++;

                if (_runningObjectID > int.MaxValue - 1)
                    _runningObjectID = 1;

                _objects[obj.ID] = obj;
            }

            return Task.CompletedTask;
        }
        public Task Leave(IFieldObj obj)
        {
            lock (_lock)
            {
                _objects.Remove(obj.ID);
            }

            return Task.CompletedTask;
        }

        public IFieldObj GetObject(int id)
        {
            if (_objects.ContainsKey(id))
                return _objects[id];
            return null;
        }

        public T GetObject<T>(int id) where T : IFieldObj
            => (T)GetObject(id);

        public IEnumerable<IFieldObj> GetObjects()
            => _objects.Values.ToImmutableList();

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => GetObjects().OfType<T>();

        public Task Dispatch(IFieldObj source, IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Where(u => u.ID != source.ID).Select(u => u.Dispatch(packet)));

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Select(u => u.Dispatch(packet)));
    }
}
