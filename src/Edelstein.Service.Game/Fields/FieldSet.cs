using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Server.FieldSet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Fields
{
    public class FieldSet : IFieldSet
    {
        public int ID => SetTemplate.ID;
        public FieldTemplate Template => Fields.FirstOrDefault()?.Template;
        public IFieldSet ParentFieldSet => this;
        public FieldSetTemplate SetTemplate { get; }
        public ICollection<IField> Fields { get; }

        public FieldSet(FieldSetTemplate template, FieldManager manager)
        {
            SetTemplate = template;
            Fields = template.Fields
                .Select(f => manager.Get(f, this))
                .ToList();
        }

        public Task Enter(IFieldObj obj)
            => Fields.FirstOrDefault()?.Enter(obj);

        public Task Leave(IFieldObj obj)
            => Fields.FirstOrDefault()?.Leave(obj);

        public IFieldObj GetObject(int id)
            => GetObjects().FirstOrDefault(o => o.ID == id);

        public T GetObject<T>(int id) where T : IFieldObj
            => GetObjects().OfType<T>().FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects()
            => Fields.SelectMany(f => f.GetObjects());

        public IEnumerable<T> GetObjects<T>() where T : IFieldObj
            => Fields.SelectMany(f => f.GetObjects<T>());

        public async Task OnUpdate(DateTime now)
        {
            //
            await Task.WhenAll(Fields.Select(f => f.OnUpdate(now)));
        }

        public Task Enter(FieldUser user, byte portal)
            => Fields.FirstOrDefault()?.Enter(user, portal);

        public Task Enter(FieldUser user, string portal)
            => Fields.FirstOrDefault()?.Enter(user, portal);

        public Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket)
            => Fields.FirstOrDefault()?.Enter(obj, getEnterPacket);

        public Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket)
            => Fields.FirstOrDefault()?.Leave(obj, getLeavePacket);

        public IFieldPool GetPool(FieldObjType type)
            => Fields.FirstOrDefault()?.GetPool(type);

        public Task BroadcastPacket(IPacket packet)
            => Task.WhenAll(Fields.Select(f => BroadcastPacket(packet)));

        public Task BroadcastPacket(IFieldObj source, IPacket packet)
            => Task.WhenAll(Fields.Select(f => BroadcastPacket(source, packet)));
    }
}