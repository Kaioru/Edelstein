using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Sets;

public class ItemSetTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IItemSetTemplate> _manager;

    public ItemSetTemplateLoader(IDataManager data, ITemplateManager<IItemSetTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("Etc/SetItemInfo.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IItemSetTemplate>(
                    id,
                    new ItemSetTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        return _manager.Count;
    }
}
