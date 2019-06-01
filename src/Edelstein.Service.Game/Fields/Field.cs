using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Fields.Generators;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields
{
    public class Field : IField
    {
        public int ID => Template.ID;
        public FieldTemplate Template { get; }

        private DateTime LastGenObjTime { get; set; }

        private readonly IDictionary<FieldObjType, IFieldPool> _pools;
        private readonly IDictionary<string, IFieldPortal> _portals;
        private readonly ICollection<IFieldGenerator> _generators;

        public Field(FieldTemplate template, ICollection<IFieldGenerator> generators)
        {
            Template = template;

            _pools = new Dictionary<FieldObjType, IFieldPool>();
            _portals = template.Portals
                .Where(kv => kv.Value.ToMap != 999999999)
                .ToDictionary(
                    kv => kv.Value.Name,
                    kv => (IFieldPortal) new FieldPortal(this, kv.Value)
                );
            _generators = generators;
        }

        public Task Enter(IFieldObj obj) => Enter(obj, null);
        public Task Leave(IFieldObj obj) => Leave(obj, null);

        public IFieldObj GetObject(int id)
            => GetObjects().FirstOrDefault(o => o.ID == id);

        public T GetObject<T>(int id) where T : IFieldObj
            => GetObjects().OfType<T>().FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects()
            => _pools.Values.SelectMany(p => p.GetObjects()).ToImmutableList();

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => _pools.Values.SelectMany(p => p.GetObjects<T>()).ToImmutableList();

        public IFieldObj GetControlledObject(IFieldUser controller, int id)
            => GetControlledObjects(controller).FirstOrDefault(o => o.ID == id);

        public T GetControlledObject<T>(IFieldUser controller, int id) where T : IFieldControlledObj
            => GetControlledObjects<T>(controller).FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetControlledObjects(IFieldUser controller)
            => controller.Controlled.ToImmutableList();

        public IEnumerable<T> GetControlledObjects<T>(IFieldUser controller) where T : IFieldControlledObj
            => controller.Controlled.OfType<T>().ToImmutableList();

        public IFieldPool GetPool(FieldObjType type)
        {
            lock (this)
            {
                if (!_pools.ContainsKey(type))
                    _pools[type] = new FieldObjPool();
                return _pools[type];
            }
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
            var first = obj is FieldUser && !pool.GetObjects().Any();

            obj.Field?.Leave(obj);
            obj.Field = this;
            await pool.Enter(obj);

            if (obj is IFieldOwnedObj owned)
                owned.Position = owned.Owner.Position;

            if (obj is FieldUser user)
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
                await Task.WhenAll(user.Pets.Select(async pet =>
                {
                    pet.Field = user.Field;
                    pet.Position = user.Position;
                    pet.Foothold = user.Foothold;
                    await user.SendPacket(pet.GetEnterFieldPacket());
                }));
                await BroadcastPacket(user, getEnterPacket?.Invoke() ?? user.GetEnterFieldPacket());

                if (!user.IsInstantiated) user.IsInstantiated = true;

                GetObjects()
                    .Where(o => o != user)
                    .ForEach(o => user.SendPacket(o.GetEnterFieldPacket()));
                await Task.WhenAll(user.Owned.Select(Enter));

                var script = first
                    ? Template.ScriptFirstUserEnter ?? Template.ScriptUserEnter
                    : Template.ScriptUserEnter;

                if (script != null)
                {
#pragma warning disable 4014
                    var context = new ConversationContext(user.Socket);
                    var conversation = await user.Service.ConversationManager.Build(
                        script,
                        context,
                        new FieldSpeaker(context, user.Field),
                        new FieldUserSpeaker(context, user)
                    );

                    user.Converse(conversation);
#pragma warning restore 4014
                }
            }
            else await BroadcastPacket(getEnterPacket?.Invoke() ?? obj.GetEnterFieldPacket());

            UpdateControlledObjects();
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null)
        {
            if (obj is FieldUser user)
            {
                await user.Dispose();
                await BroadcastPacket(user, user.GetLeaveFieldPacket());
                await Task.WhenAll(user.Owned.Select(Leave));
            }
            else await BroadcastPacket(getLeavePacket?.Invoke() ?? obj.GetLeaveFieldPacket());

            if (obj is IFieldGeneratedObj g && g.Generator != null)
                await g.Generator.Reset();
            if (obj is IFieldControlledObj c)
                c.Controller?.Controlled.Remove(c);

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
            lock (this)
            {
                var controllers = GetObjects()
                    .OfType<IFieldUser>()
                    .OrderBy(u => u.Controlled.Count)
                    .ToList();
                var controlled = GetObjects().OfType<IFieldControlledObj>().ToList();

                controlled
                    .Where(c => c.Controller == null ||
                                !controllers.Contains(c.Controller))
                    .ForEach(c => c.Controller = controllers.FirstOrDefault());
            }
        }

        public async Task OnTick(DateTime now)
        {
            if (!GetObjects<IFieldUser>().Any()) return;

            await Task.WhenAll(GetObjects()
                .OfType<ITickable>()
                .Select(o => o.OnTick(now))
            );

            if ((now - LastGenObjTime).TotalSeconds >= 7)
            {
                var generators = _generators
                    .Where(g => !(g.RegenInterval.TotalSeconds > 0) ||
                                g.Generated == null && now > g.RegenAfter)
                    .ToList();
                var mobGenerators = generators
                    .OfType<MobFieldGenerator>()
                    .ToList();
                var userCount = GetObjects<FieldUser>().Count();
                var mobCount = GetObjects<FieldMob>().Count();
                var mobCapacity = mobCount > Template.MobCapacityMin / 2
                    ? userCount < Template.MobCapacityMin * 2
                        ? Template.MobCapacityMin +
                          (Template.MobCapacityMax - Template.MobCapacityMin) *
                          (2 * userCount - Template.MobCapacityMin) /
                          (3 * Template.MobCapacityMin)
                        : Template.MobCapacityMax
                    : Template.MobCapacityMin;
                var mobGenCount = mobCapacity - mobCount;

                if (mobGenCount > 0)
                    await Task.WhenAll(mobGenerators
                        .Shuffle()
                        .Take(mobGenCount)
                        .Select(g => g.Generate(this)));

                await Task.WhenAll(generators
                    .Except(mobGenerators)
                    .Shuffle()
                    .Select(g => g.Generate(this)));

                LastGenObjTime = now;
            }
        }
    }
}