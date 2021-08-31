using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Templating;
using Foundatio.Caching;

namespace Edelstein.Common.Gameplay.Templating
{
    public class TemplateRepository<TEntry> : ITemplateRepository<TEntry> where TEntry : class, ITemplate
    {
        private readonly ICacheClient _cache;
        private readonly IDictionary<int, TemplateProvider<TEntry>> _providers;
        private readonly TimeSpan _duration;

        public int Count => _providers.Count;

        public TemplateRepository(TimeSpan duration)
        {
            _cache = new InMemoryCacheClient();
            _providers = new Dictionary<int, TemplateProvider<TEntry>>();
            _duration = duration;
        }

        public void Register(TemplateProvider<TEntry> provider)
            => _providers[provider.ID] = provider;

        public async Task<TEntry> Retrieve(int key)
        {
            if (!await _cache.ExistsAsync(key.ToString()) && _providers.ContainsKey(key))
            {
                var provider = _providers[key];
                var template = await provider.Provide();

                await _cache.SetAsync(key.ToString(), template);
            }

            var entry = await _cache.GetAsync<TEntry>(key.ToString());

            if (entry.HasValue)
            {
                await _cache.SetExpirationAsync(key.ToString(), _duration);
                return entry.Value;
            }
            return null;
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<int> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));

        public async Task<IEnumerable<TEntry>> RetrieveAll()
            => await Task.WhenAll(_providers.Keys.Select(key => Retrieve(key)));
    }
}
