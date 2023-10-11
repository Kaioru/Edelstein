using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Templates;

public class FieldStringTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IFieldStringTemplate> _manager;

    public FieldStringTemplateLoader(IDataNamespace data, ITemplateManager<IFieldStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.ResolvePath("String/Map.img");

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
                        n.Cache()
                    )
                ));
            }));

        return _manager.Count;
    }
}
