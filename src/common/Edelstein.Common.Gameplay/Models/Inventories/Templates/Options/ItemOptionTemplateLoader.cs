﻿using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Options;

public class ItemOptionTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IItemOptionTemplate> _manager;

    public ItemOptionTemplateLoader(IDataManager data, ITemplateManager<IItemOptionTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }
    
    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Item/ItemOption.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IItemOptionTemplate>(
                    id,
                    new ItemOptionTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        return _manager.Count;
    }
}
