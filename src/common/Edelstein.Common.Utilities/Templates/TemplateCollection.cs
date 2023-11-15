using System.Collections.Immutable;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateCollection<TTemplate>(
    IReadOnlyDictionary<int, TTemplate> templates
) :
    ITemplateCollection<TTemplate>
    where TTemplate : ITemplate
{
    public int Count => templates.Count;

    public Task<TTemplate?> Retrieve(int key)
        => Task.FromResult(templates.TryGetValue(key, out var result) ? result : default);
    
    public Task<ICollection<TTemplate>> RetrieveAll() 
        => Task.FromResult<ICollection<TTemplate>>(templates.Values.ToImmutableArray());
}
