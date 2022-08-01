using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Util.Templates;

public class TemplateProviderLazy<TTemplate> : ITemplateProvider<TTemplate> where TTemplate : ITemplate
{
    private readonly Func<TTemplate> _func;
    private readonly WeakReference _reference;

    public TemplateProviderLazy(int id, Func<TTemplate> func)
    {
        ID = id;
        _func = func;
        _reference = new WeakReference(null);
    }

    public int ID { get; }

    public Task<TTemplate> Provide()
        => Task.FromResult(
            ((TemplateProviderLazyHolder<TTemplate>)(_reference.Target ??=
                new TemplateProviderLazyHolder<TTemplate>(_func.Invoke())))
            .Template);
}
