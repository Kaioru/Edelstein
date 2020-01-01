using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Provider;

namespace Edelstein.Service.Game.Fields
{
    public class FieldManager : ITickBehavior
    {
        private readonly IDataTemplateManager _templateManager;
        private readonly IDictionary<int, IField> _fields;

        public FieldManager(IDataTemplateManager templateManager)
        {
            _templateManager = templateManager;
            _fields = new ConcurrentDictionary<int, IField>();
        }

        public ICollection<IField> GetAll()
        {
            lock (this)
            {
                return _fields.Values;
            }
        }

        public IField Get(int id)
        {
            lock (this)
            {
                if (_fields.ContainsKey(id)) return _fields[id];

                var template = _templateManager.Get<FieldTemplate>(id);

                if (template == null) return null;

                var field = new Field(template, _templateManager);
                _fields[id] = field;

                return _fields[id];
            }
        }

        public Task TryTick()
            => Task.WhenAll(_fields.Values.Select(f => f.TryTick()));
    }
}