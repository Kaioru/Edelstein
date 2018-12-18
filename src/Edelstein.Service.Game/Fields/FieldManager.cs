using System.Collections.Generic;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field;

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
                    _fields[id] = new Field(_templateManager.Get<FieldTemplate>(id));
                return _fields[id];
            }
        }
    }
}