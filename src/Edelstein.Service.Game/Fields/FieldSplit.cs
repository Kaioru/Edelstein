using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields
{
    public class FieldSplit : IFieldSplit
    {
        private readonly ICollection<IFieldUser> _watchers;
        private readonly ICollection<IFieldObj> _objects;

        public ICollection<IFieldUser> Watchers => _watchers.ToImmutableList();
        public int Col { get; }
        public int Row { get; }

        public FieldSplit(int col, int row)
        {
            _watchers = new List<IFieldUser>();
            _objects = new List<IFieldObj>();
            Row = row;
            Col = col;
        }

        public async Task Enter(
            IFieldObj obj,
            IFieldSplit from,
            Func<IPacket> getEnterPacket,
            Func<IPacket> getLeavePacket
        )
        {
            if (from != null)
                await from.LeaveQuietly(obj);
            await EnterQuietly(obj);

            var toWatchers = Watchers;
            var fromWatchers = from?.Watchers ?? new List<IFieldUser>();
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

            await Task.WhenAll(newWatchers.Select(w => w.SendPacket(enterPacket)));
            await Task.WhenAll(oldWatchers.Select(w => w.SendPacket(leavePacket)));

            if (obj is IFieldUser user)
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

                enclosingSplits.CopyTo(user.Watching, 0);

                await Task.WhenAll(newSplits.Select(s => s.Watch(user)));
                await Task.WhenAll(oldSplits.Select(s => s.Unwatch(user)));

                if (user.Party != null)
                {
                    using var p = new OutPacket(SendPacketOperations.UserHP);

                    p.EncodeInt(user.ID);
                    p.EncodeInt(user.Character.HP);
                    p.EncodeInt(user.Character.MaxHP);

                    await Task.WhenAll(newWatchers.Select(w => w.SendPacket(p)));
                }
            }

            UpdateControlledObjects();
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket)
        {
            await LeaveQuietly(obj);
            await BroadcastPacket(obj, getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());
            UpdateControlledObjects();
        }

        public Task EnterQuietly(IFieldObj obj)
        {
            obj.FieldSplit = this;
            lock (this) _objects.Add(obj);
            return Task.CompletedTask;
        }

        public Task LeaveQuietly(IFieldObj obj)
        {
            obj.FieldSplit = null;
            lock (this) _objects.Remove(obj);
            return Task.CompletedTask;
        }

        public async Task Watch(IFieldUser user)
        {
            lock (this) _watchers.Add(user);
            await Task.WhenAll(_objects
                .Where(o => o != user)
                .Select(o => user.SendPacket(o.GetEnterFieldPacket())));

            if (user.Party != null)
                await Task.WhenAll(GetObjects<IFieldUser>()
                    .Where(u => u.Party?.ID == user.Party.ID)
                    .Where(u => u != user)
                    .Select(async u =>
                    {
                        using var p = new OutPacket(SendPacketOperations.UserHP);

                        p.EncodeInt(u.ID);
                        p.EncodeInt(u.Character.HP);
                        p.EncodeInt(u.Character.MaxHP);

                        await user.SendPacket(p);
                    }));

            UpdateControlledObjects();
        }

        public async Task Unwatch(IFieldUser user)
        {
            lock (this) _watchers.Remove(user);
            await Task.WhenAll(_objects
                .Where(o => o != user)
                .Select(o => user.SendPacket(o.GetLeaveFieldPacket())));
            UpdateControlledObjects();
        }

        public IEnumerable<IFieldUser> GetWatchers()
            => _watchers.ToImmutableList();

        public Task BroadcastPacket(IPacket packet)
            => BroadcastPacket(null, packet);

        public Task BroadcastPacket(IFieldObj source, IPacket packet)
            => Task.WhenAll(_watchers
                .Where(w => w != source)
                .Select(w => w.SendPacket(packet)));

        public Task Enter(IFieldObj obj)
            => Enter(obj, null, null, null);

        public Task Leave(IFieldObj obj)
            => Leave(obj, null);

        public IFieldObj GetObject(int id)
            => _objects
                .FirstOrDefault(o => o.ID == id);

        public T GetObject<T>(int id) where T : IFieldObj
            => _objects
                .OfType<T>()
                .FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects()
            => _objects
                .ToImmutableList();

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => _objects
                .OfType<T>()
                .ToImmutableList();

        public IFieldObj GetControlledObject(IFieldUser controller, int id)
            => GetControlledObjects(controller).FirstOrDefault(o => o.ID == id);

        public T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlled
            => GetControlledObjects<T>(controller).FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller)
            => GetControlledObjects<IFieldControlled>(controller);

        public IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlled
            => GetObjects<T>().Where(o => o.Controller == controller).ToImmutableList();

        public void UpdateControlledObjects()
        {
            lock (this)
            {
                var controllers = _watchers
                    .OrderBy(u => u.Controlling.Count)
                    .ToImmutableList();
                var controlled = GetObjects().OfType<IFieldControlled>().ToList();

                controlled
                    .Where(c => c.Controller == null ||
                                !controllers.Contains(c.Controller))
                    .ForEach(c => c.Controller = controllers.FirstOrDefault());
            }
        }
    }
}