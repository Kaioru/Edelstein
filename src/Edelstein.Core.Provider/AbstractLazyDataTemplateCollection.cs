using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Provider
{
    public abstract class AbstractLazyDataTemplateCollection : IDataTemplateCollection
    {
        protected IDictionary<int, IDataTemplate> Templates { get; }

        protected AbstractLazyDataTemplateCollection()
            => Templates = new Dictionary<int, IDataTemplate>();

        public IDataTemplate Get(int id)
            => GetAsync(id).Result;

        public IEnumerable<IDataTemplate> GetAll()
            => Templates.Values;

        public async Task<IDataTemplate> GetAsync(int id)
        {
            if (Templates.ContainsKey(id)) return Templates[id];

            try
            {
                var template = await Load(id);

                Templates[id] = template;
                return template;
            }
            catch
            {
                Templates[id] = null;
                return null;
            }
        }

        public Task<IEnumerable<IDataTemplate>> GetAllAsync()
            => Task.FromResult(GetAll());

        public abstract Task<IDataTemplate> Load(int id);
    }
}