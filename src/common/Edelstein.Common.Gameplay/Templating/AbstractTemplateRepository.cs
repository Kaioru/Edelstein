using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Templating
{
    public abstract class AbstractTemplateRepository<TEntry> : ITemplateRepository<TEntry>, ITickerBehavior where TEntry : class, ITemplate
    {
        protected IDictionary<int, AbstractTemplateRepositoryEntry<TEntry>> Entries { get; }

        protected AbstractTemplateRepository()
            => Entries = new Dictionary<int, AbstractTemplateRepositoryEntry<TEntry>>();

        public Task<TEntry> Retrieve(int key)
        {
            if (Entries.ContainsKey(key))
                return Task.FromResult(Entries[key].Template);
            return null;
        }

        public async Task<IEnumerable<TEntry>> Retrieve(IEnumerable<int> keys)
            => await Task.WhenAll(keys.Select(key => Retrieve(key)));

        public Task<IEnumerable<TEntry>> RetrieveAll()
            => Task.FromResult(Entries.Values.Select(v => v.Template));

        public Task OnTick()
        {
            var now = DateTime.UtcNow;
            var expired = Entries.Values
                .Where(e => e.HasTemplate && now > e.DateExpire)
                .ToList();

            foreach (var e in expired) e.Reset();
            return Task.CompletedTask;
        }
    }
}
