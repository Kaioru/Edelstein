using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobTemplateLoader(
    ILogger<AbstractTemplateLoader<IMobTemplate>> logger, 
    ITemplateManagerContext<IMobTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IMobTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IMobTemplate> context)
    {
        var directory = data.ResolvePath("Mob");

        if (directory == null) return;

        await Task.WhenAll(directory.Children
            .Where(n => n.Name.Split(".")[0].All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await context.Insert(new TemplateProviderLazy<IMobTemplate>(
                    id,
                    () =>
                    {
                        var node = n.Cache();
                        return new MobTemplate(
                            id,
                            node,
                            node.ResolvePath("info")!.Cache()
                        );
                    }));
            }));
    }
}
