using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates;

public class ItemTemplateLoader(
    ILogger<AbstractTemplateLoader<IItemTemplate>> logger, 
    ITemplateManagerContext<IItemTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<IItemTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<IItemTemplate> context)
    {
        var dirCharacter = data.ResolvePath("Character")?.Cache();
        var dirItem = data.ResolvePath("Item")?.Cache();

        var nodesEquip = new List<IDataNode?>
        {
            dirCharacter?.ResolvePath("Accessory"),
            dirCharacter?.ResolvePath("Cap"),
            dirCharacter?.ResolvePath("Cape"),
            dirCharacter?.ResolvePath("Coat"),
            dirCharacter?.ResolvePath("Dragon"),
            dirCharacter?.ResolvePath("Glove"),
            dirCharacter?.ResolvePath("Longcoat"),
            dirCharacter?.ResolvePath("Mechanic"),
            dirCharacter?.ResolvePath("Pants"),
            dirCharacter?.ResolvePath("PetEquip"),
            dirCharacter?.ResolvePath("Ring"),
            dirCharacter?.ResolvePath("Shield"),
            dirCharacter?.ResolvePath("Shoes"),
            dirCharacter?.ResolvePath("TamingMob"),
            dirCharacter?.ResolvePath("Weapon")
        };
        var nodesBundle = new List<IDataNode?>
        {
            dirItem?.ResolvePath("Cash"),
            dirItem?.ResolvePath("Consume"),
            dirItem?.ResolvePath("Etc"),
            dirItem?.ResolvePath("Install")
        };
        var nodesPet = dirItem?.ResolvePath("Pet");

        var loadEquip = nodesEquip
            .Where(n => n != null)
            .SelectMany(n => n!.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                var node = n.ResolvePath("info");
                if (node == null) return;
                await context.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemEquipTemplate(id, node.Cache())
                ));
            })
            .ToImmutableList();
        var loadBundle = nodesBundle
            .Where(n => n != null)
            .SelectMany(n => n!.Children)
            .SelectMany(n => n.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                var node = n.ResolvePath("info");
                if (node == null) return;
                await context.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemBundleTemplate(id, node.Cache())
                ));
            })
            .ToImmutableList();
        var loadPet = nodesPet?
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                var node = n.ResolvePath("info");
                if (node == null) return;
                await context.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemPetTemplate(id, node.Cache())
                ));
            })
            .ToImmutableList();
        
        await Task.WhenAll(loadEquip);
        await Task.WhenAll(loadBundle);
        if (loadPet != null) await Task.WhenAll(loadPet);
    }
}
