using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Bootstrap;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Utilities.Templates;

public abstract class AbstractTemplateLoader<TTemplate>(
    ILogger<AbstractTemplateLoader<TTemplate>> logger,
    ITemplateManagerContext<TTemplate> context
) : IBootLoader
    where TTemplate : ITemplate
{
    public async Task Load()
    {
        var current = new TemplateManagerContext<TTemplate>();
        var watch = new Stopwatch();
        
        watch.Start();
        await Load(current);

        var count = current.Count;
        var elapsed = watch.Elapsed.TotalMilliseconds;

        logger.LogTemplateLoaderLoaded(GetType().Name, count, elapsed);

        await Task.WhenAll((await current.RetrieveAll()).Select(context.Insert));
    }

    protected abstract Task Load(ITemplateManagerContext<TTemplate> context);
}
