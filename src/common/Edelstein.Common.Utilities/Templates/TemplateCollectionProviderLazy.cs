using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateCollectionProviderLazy<TTemplate>(
    int id, 
    Func<TTemplate> func
) : ITemplateCollectionProvider<TTemplate>
    where TTemplate : ITemplate
{
    private TTemplate? _template = default;

    public int ID { get; } = id;

    public Task<TTemplate> Provide() 
        => Task.FromResult(_template ??= func.Invoke());
}
