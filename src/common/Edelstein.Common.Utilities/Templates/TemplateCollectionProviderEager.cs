using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateCollectionProviderEager<TTemplate>(
    int id, 
    TTemplate template
) : ITemplateCollectionProvider<TTemplate>
    where TTemplate : ITemplate
{

    public int ID { get; } = id;

    public Task<TTemplate> Provide() => Task.FromResult(template);
}
