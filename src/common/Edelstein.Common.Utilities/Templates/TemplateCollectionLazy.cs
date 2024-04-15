using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateCollectionLazy<TTemplate>(
    IReadOnlyDictionary<int, ITemplateCollectionProvider<TTemplate>> providers
) :
    ITemplateCollection<TTemplate>
    where TTemplate : ITemplate
{
    public int Count => providers.Count;

    public async Task<TTemplate?> Retrieve(int key)
        => providers.TryGetValue(key, out var provider) ? await provider.Provide() : default;

    public async Task<ICollection<TTemplate>> RetrieveAll()
        => await Task.WhenAll(providers.Values.Select(p => p.Provide()));
}
