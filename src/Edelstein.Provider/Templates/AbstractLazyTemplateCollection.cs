using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates
{
    public abstract class AbstractLazyTemplateCollection : ITemplateCollection
    {
        protected IDataDirectoryCollection Collection { get; }
        protected IDictionary<int, ITemplate> Templates { get; }
        public IEnumerable<ITemplate> Cache => Templates.Values;

        protected AbstractLazyTemplateCollection(IDataDirectoryCollection collection)
        {
            Collection = collection;
            Templates = new Dictionary<int, ITemplate>();
        }

        public ITemplate Get(int id)
        {
            if (!Templates.ContainsKey(id))
                Templates[id] = Load(id).Result;
            return Templates[id];
        }

        public IEnumerable<ITemplate> GetAll()
            => Templates.Values;

        public Task<ITemplate> GetAsync(int id)
            => Task.Run(() => Get(id));

        public abstract Task<ITemplate> Load(int id);
    }
}