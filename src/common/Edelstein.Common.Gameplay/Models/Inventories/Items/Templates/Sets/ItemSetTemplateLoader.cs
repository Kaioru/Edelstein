using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates.Sets;

public class ItemSetTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IItemSetTemplate> _manager;

    public ItemSetTemplateLoader(IDataNamespace data, ITemplateManager<IItemSetTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Etc/SetItemInfo.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IItemSetTemplate>(
                    id,
                    new ItemSetTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        _manager.Freeze();
        return _manager.Count;
    }
}
