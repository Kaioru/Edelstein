using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class FieldSplit : IFieldSplit
    {
        public int Row { get; }
        public int Col { get; }

        private readonly IDictionary<int, IFieldObjUser> _watchers;
        private readonly IDictionary<int, IFieldObj> _objects;
        private readonly object _watcherLock;
        private readonly object _objectLock;

        public FieldSplit(int row, int col)
        {
            Row = row;
            Col = col;

            _watchers = new Dictionary<int, IFieldObjUser>();
            _objects = new Dictionary<int, IFieldObj>();
            _watcherLock = new object();
            _objectLock = new object();
        }

        public async Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null, Func<IPacket> getLeavePacket = null)
        {
            var from = obj.FieldSplit;

            if (from != null)
                await from.LeaveQuietly(obj);
            await EnterQuietly(obj);

            var toWatchers = GetWatchers();
            var fromWatchers = from?.GetWatchers() ?? new List<IFieldObjUser>();
            var newWatchers = toWatchers
                .Where(w => w != obj)
                .Except(fromWatchers)
                .ToImmutableList();
            var oldWatchers = fromWatchers
                .Where(w => w != obj)
                .Except(toWatchers)
                .ToImmutableList();

            var enterPacket = getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket();
            var leavePacket = getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket();

            await Task.WhenAll(newWatchers.Select(w => w.Dispatch(enterPacket)));
            await Task.WhenAll(oldWatchers.Select(w => w.Dispatch(leavePacket)));

            if (obj is IFieldObjUser user)
            {
                var enclosingSplits = user.Field.GetEnclosingSplits(this);
                var oldSplits = user.Watching
                    .Where(s => s != null)
                    .Except(enclosingSplits)
                    .ToImmutableArray();
                var newSplits = enclosingSplits
                    .Where(s => s != null)
                    .Except(user.Watching)
                    .ToImmutableArray();

                await Task.WhenAll(newSplits.Select(s => s.Watch(user)));
                await Task.WhenAll(oldSplits.Select(s => s.Unwatch(user)));
            }

            UpdateControlledObjects();
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null)
        {
            await LeaveQuietly(obj);
            await Dispatch(obj, getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

            UpdateControlledObjects();
        }

        public Task Enter(IFieldObj obj) => Enter(obj, null, null);
        public Task Leave(IFieldObj obj) => Leave(obj, null);

        public Task EnterQuietly(IFieldObj obj)
        {
            lock (_objectLock)
            {
                _objects[obj.ID] = obj;
                obj.FieldSplit = this;
            }

            return Task.CompletedTask;
        }
        public Task LeaveQuietly(IFieldObj obj)
        {
            lock (_objectLock)
            {
                _objects.Remove(obj.ID);
                obj.FieldSplit = null;
            }

            return Task.CompletedTask;
        }

        public async Task Watch(IFieldObjUser user)
        {

            lock (_watcherLock)
            {
                _watchers[user.ID] = user;
                user.Watching.Add(this);
            }

            await Task.WhenAll(GetObjects()
                .Where(o => o != user)
                .Select(o => user.Dispatch(o.GetEnterFieldPacket())));

            UpdateControlledObjects();
        }

        public async Task Unwatch(IFieldObjUser user)
        {
            lock (_watcherLock)
            {
                _watchers.Remove(user.ID);
                user.Watching.Remove(this);
            }

            await Task.WhenAll(GetObjects()
                .Where(o => o != user)
                .Select(o => user.Dispatch(o.GetLeaveFieldPacket())));

            UpdateControlledObjects();
        }

        private void UpdateControlledObjects()
        {
            lock (_watcherLock)
            {
                var controllers = GetWatchers()
                    .OrderBy(u => u.Controlling.Count)
                    .ToImmutableList();
                var controlled = GetObjects().OfType<IFieldControlledObj>().ToList();

                controlled
                    .Where(c => c.Controller == null ||
                                !controllers.Contains(c.Controller))
                    .ForEach(c => c.Controller = controllers.FirstOrDefault());
            }
        }

        public IFieldObjUser GetWatcher(int id)
        {
            if (_watchers.ContainsKey(id))
                return _watchers[id];
            return null;
        }

        public IEnumerable<IFieldObjUser> GetWatchers()
            => _watchers.Values.ToImmutableList();

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
            => Task.WhenAll(GetWatchers().Where(u => u.ID != source.ID).Select(u => u.Dispatch(packet)));

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(GetWatchers().Select(u => u.Dispatch(packet)));
    }
}
