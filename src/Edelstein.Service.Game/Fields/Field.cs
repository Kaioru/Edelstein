using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Interactions.Miniroom;
using Edelstein.Service.Game.Interactions.Miniroom.Trade;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields
{
    public class Field : IField
    {
        public int ID => Template.ID;
        public FieldTemplate Template { get; }
        private readonly IDictionary<FieldObjType, IFieldPool> _pools;

        public Field(FieldTemplate template)
        {
            Template = template;
            _pools = new Dictionary<FieldObjType, IFieldPool>();
        }

        public Task Enter(IFieldObj obj) => Enter(obj, null);
        public Task Leave(IFieldObj obj) => Leave(obj, null);

        public async Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket)
        {
            var pool = GetPool(obj.Type);

            obj.Field?.Leave(obj);
            obj.Field = this;
            await pool.Enter(obj);

            if (obj is FieldUser user)
            {
                var portal = Template.Portals.Values.FirstOrDefault(p => p.ID == user.Character.FieldPortal) ??
                             Template.Portals.Values.First(p => p.Type == FieldPortalType.Spawn);

                user.ID = user.Character.ID;
                user.Character.FieldID = ID;
                user.Position = portal.Position;

                if (portal.Type != FieldPortalType.Spawn)
                {
                    var foothold = Template.Footholds.Values
                        .Where(f => f.X1 <= portal.Position.X && f.X2 >= portal.Position.X)
                        .First(f => f.X1 < f.X2);

                    user.Foothold = (short) foothold.ID;
                }

                await user.SendPacket(user.GetSetFieldPacket());
                await BroadcastPacket(user, getEnterPacket?.Invoke() ?? user.GetEnterFieldPacket());

                if (!user.Socket.IsInstantiated) user.Socket.IsInstantiated = true;

                await user.ResetForcedStats();
                GetObjects()
                    .Where(o => o != user)
                    .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
            }
            else await BroadcastPacket(getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket());

            UpdateControlledObjects();
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket)
        {
            if (obj is FieldUser user)
            {
                if (user.Dialog != null)
                {
                    if (user.Dialog is TradingRoom trade) await trade.Close();
                    else if (user.Dialog is IMiniRoom room) await room.Leave(user);
                    else await user.Interact(user.Dialog, true);
                }

                await BroadcastPacket(user, user.GetLeaveFieldPacket());
            }
            else await BroadcastPacket(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

            await GetPool(obj.Type).Leave(obj);
            UpdateControlledObjects();
        }

        public void UpdateControlledObjects()
        {
            var controllers = GetObjects().OfType<FieldUser>().Shuffle().ToList();
            var controlled = GetObjects().OfType<AbstractFieldControlledLife>().ToList();

            controlled
                .Where(
                    c => c.Controller == null ||
                         !controllers.Contains(c.Controller))
                .ForEach(c => c.ChangeController(controllers.FirstOrDefault()));
        }

        public IFieldPool GetPool(FieldObjType type)
        {
            if (!_pools.ContainsKey(type))
                _pools[type] = new FieldObjPool();
            return _pools[type];
        }

        public IFieldObj GetObject(int id)
            => GetObjects().FirstOrDefault(o => o.ID == id);

        public T GetObject<T>(int id) where T : IFieldObj
            => GetObjects().OfType<T>().FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects()
            => _pools.Values.SelectMany(p => p.GetObjects());

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => GetObjects().OfType<T>();

        public Task BroadcastPacket(IPacket packet)
            => BroadcastPacket(null, packet);

        public Task BroadcastPacket(IFieldObj source, IPacket packet)
            => Task.WhenAll(
                GetObjects<FieldUser>()
                    .Where(u => u != source)
                    .Select(u => u.SendPacket(packet))
            );
    }
}