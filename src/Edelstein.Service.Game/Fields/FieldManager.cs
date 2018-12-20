using System;
using System.Collections.Generic;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.NPC;
using Edelstein.Service.Game.Fields.Objects;
using MoreLinq;

namespace Edelstein.Service.Game.Fields
{
    public class FieldManager
    {
        private readonly ITemplateManager _templateManager;
        private readonly IDictionary<int, Field> _fields;

        public FieldManager(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
            _fields = new Dictionary<int, Field>();
        }

        public IField Get(int id)
        {
            lock (this)
            {
                if (!_fields.ContainsKey(id))
                {
                    var field = new Field(_templateManager.Get<FieldTemplate>(id));

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
    }
}