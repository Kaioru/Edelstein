using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Templates;

public class FieldTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IFieldTemplate> _manager;

    public FieldTemplateLoader(IDataNamespace data, ITemplateManager<IFieldTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.ResolvePath("Map/Map");

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .Where(n => n.Name.StartsWith("Map"))
            .SelectMany(n => n.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await _manager.Insert(new TemplateProviderLazy<IFieldTemplate>(
                    id,
                    () => new FieldTemplate(
                        id,
                        n.ResolvePath("foothold")!.ResolveAll(),
                        n.ResolvePath("portal")!.ResolveAll(),
                        n.ResolvePath("ladderRope")!.ResolveAll(),
                        n.ResolvePath("life")!.ResolveAll(),
                        n.ResolvePath("info")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
