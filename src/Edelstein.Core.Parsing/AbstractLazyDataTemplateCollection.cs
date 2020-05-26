using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Provider
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
            _templates.TryGetValue(id, out var template);

            try
            {
                if (template == null)
                {
                    template = await Load(id);
                    _templates[id] = template;
                }
            }
            catch
            {
                // ignored
            }

            return template;
        }

        public Task<IEnumerable<IDataTemplate>> GetAllAsync()
            => Task.FromResult(GetAll());

        public abstract Task<IDataTemplate> Load(int id);
    }
}