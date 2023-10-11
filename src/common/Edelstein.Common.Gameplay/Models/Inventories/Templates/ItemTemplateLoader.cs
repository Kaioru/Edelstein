using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public class ItemTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IItemTemplate> _manager;
    
    public ItemTemplateLoader(IDataNamespace data, ITemplateManager<IItemTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var dirCharacter = _data.ResolvePath("Character")?.Cache();
        var dirItem = _data.ResolvePath("Item")?.Cache();

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
                var node = n.ResolvePath("info")?.Cache();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemEquipTemplate(id, node)
                ));
            })
            .ToFrozenSet();
        var loadBundle = nodesBundle
            .Where(n => n != null)
            .SelectMany(n => n!.Children)
            .SelectMany(n => n.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                var node = n.ResolvePath("info")?.Cache();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemBundleTemplate(id, node)
                ));
            })
            .ToFrozenSet();
        var loadPet = nodesPet?
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                var node = n.ResolvePath("info")?.Cache();
                if (node == null) return;
                await _manager.Insert(new TemplateProviderLazy<IItemTemplate>(
                    id,
                    () => new ItemPetTemplate(id, node)
                ));
            })
            .ToFrozenSet();

        await Task.WhenAll(loadEquip);
        await Task.WhenAll(loadBundle);
        if (loadPet != null) await Task.WhenAll(loadPet);

        _manager.Freeze();
        return _manager.Count;
    }
}
