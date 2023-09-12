using System.Collections.Immutable;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public class ItemTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IItemTemplate> _manager;
    
    public ItemTemplateLoader(IDataManager data, ITemplateManager<IItemTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var dirCharacter = _data.Resolve("Character")?.ResolveAll();
        var dirItem = _data.Resolve("Item")?.ResolveAll();

        var nodesEquip = new List<IDataNode?>
        {
            dirCharacter?.Resolve("Accessory"),
            dirCharacter?.Resolve("Cap"),
            dirCharacter?.Resolve("Cape"),
            dirCharacter?.Resolve("Coat"),
            dirCharacter?.Resolve("Dragon"),
            dirCharacter?.Resolve("Glove"),
            dirCharacter?.Resolve("Longcoat"),
            dirCharacter?.Resolve("Mechanic"),
            dirCharacter?.Resolve("Pants"),
            dirCharacter?.Resolve("PetEquip"),
            dirCharacter?.Resolve("Ring"),
            dirCharacter?.Resolve("Shield"),
            dirCharacter?.Resolve("Shoes"),
            dirCharacter?.Resolve("TamingMob"),
            dirCharacter?.Resolve("Weapon")
        };
        var nodesBundle = new List<IDataNode?>
        {
            dirItem?.Resolve("Cash"),
            dirItem?.Resolve("Consume"),
            dirItem?.Resolve("Etc"),
            dirItem?.Resolve("Install")
        };
        var nodesPet = dirItem?.Resolve("Pet");

        var loadEquip = nodesEquip
            .Where(n => n != null)
            .SelectMany(n => n!.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                var node = n.Resolve("info")?.ResolveAll();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemEquipTemplate(id, node)
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
                var node = n.Resolve("info")?.ResolveAll();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemBundleTemplate(id, node)
                ));
            })
            .ToImmutableList();
        var loadPet = nodesPet?
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                var node = n.Resolve("info")?.ResolveAll();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemPetTemplate(id, node)
                ));
            })
            .ToImmutableList();

        await Task.WhenAll(loadEquip);
        await Task.WhenAll(loadBundle);
        if (loadPet != null) await Task.WhenAll(loadPet);

        return _manager.Count;
    }
}
