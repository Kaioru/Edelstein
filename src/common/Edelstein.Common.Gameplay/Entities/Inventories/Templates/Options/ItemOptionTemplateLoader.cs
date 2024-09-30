using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates.Options;

public class ItemOptionTemplateLoader(
    ILogger<AbstractTemplateLoader<IItemOptionTemplate>> logger,
    ITemplateManagerContext<IItemOptionTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IItemOptionTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IItemOptionTemplate> context)
    {
        var dir = data.ResolvePath("Item/ItemOption.img");

        if (dir == null) return;

        await Task.WhenAll(dir.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await context.Insert(new TemplateProviderEager<IItemOptionTemplate>(
                    id,
                    new ItemOptionTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }));
    }
}
