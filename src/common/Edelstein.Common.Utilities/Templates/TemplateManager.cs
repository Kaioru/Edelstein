using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateManager<TTemplate> : ITemplateManager<TTemplate>
    where TTemplate : ITemplate
{
    private readonly IDictionary<int, ITemplateProvider<TTemplate>> _dictionary;
    public int Count => _dictionary.Count;

    public TemplateManager(ITemplateManagerContext<TTemplate> context)
        => _dictionary = context.RetrieveAll().Result.ToFrozenDictionary(
            t => t.ID,
            t => t
        );

    public async Task<TTemplate?> Retrieve(int key) 
        => _dictionary.TryGetValue(key, out var provider) ? await provider.Provide() : default;

    public async Task<ICollection<TTemplate>> RetrieveAll() 
        => await Task.WhenAll(_dictionary.Values.Select(p => p.Provide()));

}
