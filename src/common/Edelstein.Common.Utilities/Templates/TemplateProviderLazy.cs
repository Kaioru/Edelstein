using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateProviderLazy<TTemplate>(
    int id, 
    Func<TTemplate> func
) : ITemplateProvider<TTemplate>
    where TTemplate : ITemplate
{
    public int ID { get; } = id;
    private TTemplate? _template;
    
    public Task<TTemplate> Provide() 
        => Task.FromResult(_template ??= func.Invoke());
}
