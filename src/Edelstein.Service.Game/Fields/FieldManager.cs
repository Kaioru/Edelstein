using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.NPC;
using Edelstein.Service.Game.Fields.Objects;
using MoreLinq;

namespace Edelstein.Service.Game.Fields
{
    public class FieldManager : IUpdateable
    {
        private readonly ITemplateManager _templateManager;
        private readonly IDictionary<int, Field> _fields;

        public FieldManager(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
            _fields = new Dictionary<int, Field>();
        }

        public IField Get(int id, IFieldSet parentFieldSet = null)
        {
            lock (this)
            {
                if (!_fields.ContainsKey(id))
                {
                    var template = _templateManager.Get<FieldTemplate>(id);

                    if (template == null) return null;

                    var field = new Field(template, parentFieldSet);

                    field.Template.Life.ForEach(l =>
                    {
                        switch (l.Type)
                        {
                            case FieldLifeType.NPC:
                                field.Enter(new FieldNPC(_templateManager.Get<NPCTemplate>(l.ID))
                                {
                                    Position = l.Position,
                                    MoveAction = l.F,
                                    Foothold = (short) l.FH,
                                    RX0 = l.RX0,
                                    RX1 = l.RX1
                                });
                                break;
                            case FieldLifeType.Monster:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    });
                    _fields[id] = field;
                }

                return _fields[id];
            }
        }

        public Task OnUpdate(DateTime now)
            => Task.WhenAll(_fields.Values.Select(f => f.OnUpdate(now)));
    }
}