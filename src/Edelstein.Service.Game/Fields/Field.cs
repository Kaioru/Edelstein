using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields
{
    public class Field : IField
    {
        public int ID => Template.ID;
        public FieldTemplate Template { get; }

        private readonly IDictionary<FieldObjType, IFieldPool> _pools;
        private readonly IDictionary<string, IFieldPortal> _portals;

        public Field(FieldTemplate template)
        {
            Template = template;
            _pools = new Dictionary<FieldObjType, IFieldPool>();
            _portals = template.Portals
                .Where(kv => kv.Value.ToMap != 999999999)
                .ToDictionary(
                    kv => kv.Value.Name,
                    kv => (IFieldPortal) new FieldPortal(this, kv.Value)
                );
        }

        public Task Enter(IFieldObj obj) => Enter(obj, null);
        public Task Leave(IFieldObj obj) => Leave(obj, null);

        public IFieldObj GetObject(int id)
            => GetObjects().FirstOrDefault(o => o.ID == id);

        public T GetObject<T>(int id) where T : IFieldObj
            => GetObjects().OfType<T>().FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects()
            => _pools.Values.SelectMany(p => p.GetObjects());

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => _pools.Values.SelectMany(p => p.GetObjects<T>());

        public IFieldObj GetControlledObject(IFieldUser controller, int id)
            => GetControlledObjects(controller).FirstOrDefault(o => o.ID == id);

        public T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlledObj
            => GetControlledObjects<T>(controller).FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller)
            => _pools.Values.SelectMany(p => p.GetControlledObjects(controller));

        public IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlledObj
            => _pools.Values.SelectMany(p => p.GetControlledObjects<T>(controller));

        public IFieldPool GetPool(FieldObjType type)
        {
            if (!_pools.ContainsKey(type))
                _pools[type] = new FieldObjPool();
            return _pools[type];
        }

        public IFieldPortal GetPortal(byte portal)
            => _portals[Template.Portals[portal].Name];

        public IFieldPortal GetPortal(string portal)
            => _portals[portal];

        public Task Enter(IFieldUser user, byte portal, Func<IPacket> getEnterPacket = null)
        {
            user.Character.FieldPortal = portal;
            return Enter(user, getEnterPacket);
        }

        public Task Enter(IFieldUser user, string portal, Func<IPacket> getEnterPacket = null)
        {
            user.Character.FieldPortal = (byte) (Template.Portals.Values
                                                     .FirstOrDefault(p => p.Name.Equals(portal))?
                                                     .ID ?? 0);
            return Enter(user, getEnterPacket);
        }

        public async Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null)
        {
            var pool = GetPool(obj.Type);

            obj.Field?.Leave(obj);
            obj.Field = this;
            await pool.Enter(obj);

            if (obj is IFieldUser user)
            {
                var portal = Template.Portals.Values.FirstOrDefault(p => p.ID == user.Character.FieldPortal) ??
                             Template.Portals.Values.First(p => p.Type == FieldPortalType.StartPoint);

                user.ID = user.Character.ID;
                user.Character.FieldID = ID;
                user.Position = portal.Position;
                user.Foothold = (short) (portal.Type != FieldPortalType.StartPoint
                    ? Template.Footholds.Values
                        .Where(f => f.X1 <= portal.Position.X && f.X2 >= portal.Position.X)
                        .First(f => f.X1 < f.X2).ID
                    : 0);

                await user.SendPacket(user.GetSetFieldPacket());
                await BroadcastPacket(user, getEnterPacket?.Invoke() ?? user.GetEnterFieldPacket());

                if (!user.IsInstantiated) user.IsInstantiated = true;

                GetObjects()
                    .Where(o => o != user)
                    .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
            }
            else await BroadcastPacket(getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket());

            UpdateControlledObjects();
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null)
        {
            if (obj is IFieldUser user)
            {
                user.Dispose();
                await BroadcastPacket(user, user.GetLeaveFieldPacket());
            }
            else await BroadcastPacket(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

            await GetPool(obj.Type).Leave(obj);
            UpdateControlledObjects();
        }

        public Task BroadcastPacket(IPacket packet)
            => BroadcastPacket(null, packet);

        public Task BroadcastPacket(IFieldObj source, IPacket packet)
            => Task.WhenAll(
                GetObjects<IFieldUser>()
                    .Where(u => u != source)
                    .Select(u => u.SendPacket(packet))
            );

        public void UpdateControlledObjects()
        {
            var controllers = GetObjects().OfType<IFieldUser>().Shuffle().ToList();
            var controlled = GetObjects().OfType<AbstractFieldControlledLife>().ToList();

            controlled
                .Where(c => c.Controller == null ||
                            !controllers.Contains(c.Controller))
                .ForEach(c => c.Controller = controllers.FirstOrDefault());
        }
    }
}