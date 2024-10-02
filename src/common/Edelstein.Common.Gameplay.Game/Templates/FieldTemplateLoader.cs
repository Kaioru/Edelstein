using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Templates;

public class FieldTemplateLoader(
    ILogger<AbstractTemplateLoader<IFieldTemplate>> logger, 
    ITemplateManagerContext<IFieldTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IFieldTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IFieldTemplate> context)
    {
        var directory = data.ResolvePath("Map/Map");

        if (directory == null) return;
        
        await Task.WhenAll(directory.Children
            .Where(n => n.Name.StartsWith("Map"))
            .SelectMany(n => n.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await context.Insert(new TemplateProviderLazy<IFieldTemplate>(
                    id,
                    () => new FieldTemplate(
                        id,
                        n.ResolvePath("foothold")!.Cache(),
                        n.ResolvePath("portal")!.Cache(),
                        n.ResolvePath("ladderRope")?.Cache(),
                        n.ResolvePath("life")?.Cache(),
                        n.ResolvePath("reactor")?.Cache(),
                        n.ResolvePath("info")!.Cache()
                    )
                ));
            }));
    }
}
