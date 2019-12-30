using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider
{
    public abstract class AbstractLazyDataTemplateCollection : IDataTemplateCollection
    {
        private readonly IDictionary<int, IDataTemplate> _templates;

        protected AbstractLazyDataTemplateCollection()
            => _templates = new Dictionary<int, IDataTemplate>();

        public IDataTemplate Get(int id)
            => GetAsync(id).Result;

        public IEnumerable<IDataTemplate> GetAll()
            => _templates.Values;

        public async Task<IDataTemplate> GetAsync(int id)
        {
            if (_templates.ContainsKey(id)) return _templates[id];

            try
            {
                var template = await Load(id);

                _templates[id] = template;
                return template;
            }
            catch
            {
                _templates[id] = null;
                return null;
            }
        }

        public Task<IEnumerable<IDataTemplate>> GetAllAsync()
            => Task.FromResult(GetAll());

        public abstract Task<IDataTemplate> Load(int id);
    }
}