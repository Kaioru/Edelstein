using System.Collections.Generic;
using System.Threading.Tasks;
using MoreLinq;

namespace Edelstein.Provider
{
    public abstract class AbstractEagerDataTemplateCollection : IDataTemplateCollection
    {
        private readonly IDictionary<int, IDataTemplate> _templates;

        protected AbstractEagerDataTemplateCollection()
            => _templates = new Dictionary<int, IDataTemplate>();

        public IDataTemplate Get(int id)
            => _templates.ContainsKey(id) ? _templates[id] : null;

        public IEnumerable<IDataTemplate> GetAll()
            => _templates.Values;

        public Task<IDataTemplate> GetAsync(int id)
            => Task.FromResult(Get(id));

        public Task<IEnumerable<IDataTemplate>> GetAllAsync()
            => Task.FromResult(GetAll());

        public async Task Populate()
        {
            (await Load())
                .DistinctBy(t => t.ID)
                .ForEach(t => _templates.Add(t.ID, t));
        }

        protected abstract Task<IEnumerable<IDataTemplate>> Load();
    }
}