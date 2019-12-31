using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public class FieldSplit : IFieldSplit
    {
        private readonly ICollection<IFieldObj> _objects;

        public int Col { get; }
        public int Row { get; }

        public FieldSplit(int col, int row)
        {
            _objects = new List<IFieldObj>();
            Row = row;
            Col = col;
        }

        public async Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket)
        {
            lock (this) _objects.Add(obj);

            var enterPacket = getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket();

            await Task.WhenAll(GetObjects<IFieldUser>()
                .Select(u => u.SendPacket(enterPacket)));
            if (obj is IFieldUser user)
                await Task.WhenAll(GetObjects()
                    .Where(o => o != obj)
                    .Select(o => user.SendPacket(o.GetEnterFieldPacket())));
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket)
        {
            lock (this) _objects.Remove(obj);

            var leavePacket = getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket();

            await Task.WhenAll(GetObjects<IFieldUser>()
                .Select(u => u.SendPacket(leavePacket)));
            if (obj is IFieldUser user)
                await Task.WhenAll(GetObjects()
                    .Where(o => o != obj)
                    .Select(o => user.SendPacket(o.GetLeaveFieldPacket())));
        }

        public Task Enter(IFieldObj obj)
            => Enter(obj, null);

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
    }
}