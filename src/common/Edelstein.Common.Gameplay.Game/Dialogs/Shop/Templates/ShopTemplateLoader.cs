using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Shop.Templates;

public class ShopTemplateLoader(
    ILogger<AbstractTemplateLoader<IShopTemplate>> logger,
    ITemplateManagerContext<IShopTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IShopTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IShopTemplate> context)
    {
        var directory = data.ResolvePath("Server/NpcShop.img");

        if (directory == null) return;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await context.Insert(new TemplateProviderLazy<IShopTemplate>(
                    id,
                    () => new ShopTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }));
    }
}
