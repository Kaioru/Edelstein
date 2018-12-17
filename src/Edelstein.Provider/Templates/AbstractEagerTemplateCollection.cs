using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates
{
    public abstract class AbstractEagerTemplateCollection : ITemplateCollection
    {
        protected IDataDirectoryCollection Collection { get; }
        protected IDictionary<int, ITemplate> Templates { get; }
        public IEnumerable<ITemplate> Cache => Templates.Values;

        public AbstractEagerTemplateCollection(IDataDirectoryCollection collection)
        {
            Collection = collection;
            Templates = new Dictionary<int, ITemplate>();
        }

        public ITemplate Get(int id)
            => Templates[id];

        public Task<ITemplate> GetAsync(int id)
            => Task.Run(() => Get(id));

        public abstract Task LoadAll();
    }
}