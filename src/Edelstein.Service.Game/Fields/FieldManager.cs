using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.Field.Life.NPC;
using Edelstein.Provider.Templates.Field.Reactor;
using Edelstein.Service.Game.Fields.Generators;
using Edelstein.Service.Game.Fields.Objects.NPC;
using MoreLinq;

namespace Edelstein.Service.Game.Fields
{
    public class FieldManager : ITickable
    {
        private readonly ITemplateManager _templateManager;
        private readonly IDictionary<int, IField> _fields;

        public FieldManager(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
            _fields = new Dictionary<int, IField>();
        }

        public IField Get(int id)
        {
            lock (this)
            {
                if (!_fields.ContainsKey(id))
                {
                    var template = _templateManager.Get<FieldTemplate>(id);

                    if (template == null) return null;

                    var generators = new List<IFieldGenerator>();
                    var field = new Field(template, generators);

                    template.Life.ForEach(l =>
                    {
                        switch (l.Type)
                        {
                            case FieldLifeType.NPC:
                                field.Enter(new FieldNPC(_templateManager.Get<NPCTemplate>(l.ID), l.Left)
                                {
                                    Position = l.Position,
                                    Foothold = (short) l.FH,
                                    RX0 = l.RX0,
                                    RX1 = l.RX1
                                });
                                break;
                            case FieldLifeType.Monster:
                                generators.Add(new MobFieldGenerator(
                                    l,
                                    _templateManager.Get<MobTemplate>(l.ID)
                                ));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    });

                    template.Reactors.ForEach(r =>
                    {
                        generators.Add(new ReactorFieldGenerator(
                            r,
                            _templateManager.Get<ReactorTemplate>(r.ID)
                        ));
                    });

                    _fields[id] = field;
                }

                return _fields[id];
            }
        }

        public Task OnTick(DateTime now)
            => Task.WhenAll(_fields.Values.Select(f => f.OnTick(now)));
    }
}