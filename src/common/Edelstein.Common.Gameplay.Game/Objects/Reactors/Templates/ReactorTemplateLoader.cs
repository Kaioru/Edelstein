using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Objects.Reactors.Templates;

public class ReactorTemplateLoader(
    ILogger<AbstractTemplateLoader<IReactorTemplate>> logger, 
    ITemplateManagerContext<IReactorTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IReactorTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IReactorTemplate> context)
    {
        var directory = data.ResolvePath("Reactor");

        if (directory == null) return;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await context.Insert(new TemplateProviderLazy<IReactorTemplate>(
                    id,
                    () =>
                    {
                        var node = n.Cache();
                        return new ReactorTemplate(
                            id,
                            node.Cache()
                        );
                    }));
            }));
    }
}
