using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Continent.Templates;

public class ContiMoveTemplateLoader(
    ILogger<AbstractTemplateLoader<IContiMoveTemplate>> logger,
    ITemplateManagerContext<IContiMoveTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IContiMoveTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IContiMoveTemplate> context)
    {
        var directory = data.ResolvePath("Server/Continent.img");

        if (directory == null) return;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await context.Insert(new TemplateProviderLazy<IContiMoveTemplate>(
                    id,
                    () => new ContiMoveTemplate(
                        id,
                        n.Cache(),
                        n.ResolvePath("field")!.Cache(),
                        n.ResolvePath("scheduler")!.Cache(),
                        n.ResolvePath("genMob")?.Cache(),
                        n.ResolvePath("reactor")?.Cache(),
                        n.ResolvePath("time")!.Cache()
                    )
                ));
            }));
    }
}
