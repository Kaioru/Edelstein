using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Templates;

public class FieldStringTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IFieldStringTemplate> _manager;

    public FieldStringTemplateLoader(IDataManager data, ITemplateManager<IFieldStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("String/Map.img");

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .SelectMany(n => n.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IFieldStringTemplate>(
                    id,
                    new FieldStringTemplate(
                        id,
                        n.ResolveAll()
                    )
                ));
            }));

        _manager.Freeze();
        return _manager.Count;
    }
}
