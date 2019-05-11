using System.Collections.Generic;

namespace Edelstein.Provider.Templates
{
    public abstract class AbstractLazyTemplateCollection : ITemplateCollection
    {
        public abstract TemplateCollectionType Type { get; }
        public IDataDirectoryCollection Collection { get; }
        private readonly IDictionary<int, ITemplate> _templates;

        public AbstractLazyTemplateCollection(IDataDirectoryCollection collection)
        {
            Collection = collection;
            _templates = new Dictionary<int, ITemplate>();
        }

        public ITemplate Get(int id)
        {
            lock (this)
            {
                if (!_templates.ContainsKey(id))
                    _templates[id] = Load(id);
                return !_templates.ContainsKey(id) ? null : _templates[id];
            }
        }

        public IEnumerable<ITemplate> GetAll()
        {
            lock (this)
            {
                return _templates.Values;
            }
        }

        public abstract ITemplate Load(int id);
    }
}