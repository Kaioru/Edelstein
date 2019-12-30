using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider
{
    public abstract class AbstractEagerDataTemplateCollection : IDataTemplateCollection
    {
        protected IDictionary<int, IDataTemplate> Templates { get; }

        protected AbstractEagerDataTemplateCollection()
            => Templates = new Dictionary<int, IDataTemplate>();

        public IDataTemplate Get(int id)
            => Templates.ContainsKey(id) ? Templates[id] : null;

        public IEnumerable<IDataTemplate> GetAll()
            => Templates.Values;

        public Task<IDataTemplate> GetAsync(int id)
            => Task.FromResult(Get(id));

        public Task<IEnumerable<IDataTemplate>> GetAllAsync()
            => Task.FromResult(GetAll());

        public abstract Task Populate();
    }
}