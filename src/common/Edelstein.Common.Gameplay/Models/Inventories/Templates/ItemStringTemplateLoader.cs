using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public class ItemStringTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IItemStringTemplate> _manager;

    public ItemStringTemplateLoader(IDataManager data, ITemplateManager<IItemStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.Resolve("String/Eqp.img")?.Children
            .SelectMany(n => n.Children)
            .SelectMany(n => n.Children)
            .Where(c => c.Name.All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IItemStringTemplate>(
                    id,
                    new ItemStringTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        await Task.WhenAll(_data.Resolve("String/Etc.img")?.Children
            .SelectMany(n => n.Children)
            .Where(c => c.Name.All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IItemStringTemplate>(
                    id,
                    new ItemStringTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        foreach (var sub in new[] { "Consume", "Ins", "Cash" })
            await Task.WhenAll(_data.Resolve($"String/{sub}.img")?.Children
                .Where(c => c.Name.All(char.IsDigit))
                .Select(async n =>
                {
                    var id = Convert.ToInt32(n.Name);
                    await _manager.Insert(new TemplateProviderEager<IItemStringTemplate>(
                        id,
                        new ItemStringTemplate(
                            id,
                            n.ResolveAll()
                        )
                    ));
                }) ?? Array.Empty<Task>());

        _manager.Freeze();
        return _manager.Count;
    }
}
