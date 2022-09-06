using Edelstein.Common.Util.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates;

public class FieldTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IFieldTemplate> _manager;

    public FieldTemplateLoader(IDataManager data, ITemplateManager<IFieldTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("Map/Map");

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
                        n.Resolve("foothold")!.ResolveAll(),
                        n.Resolve("portal")!.ResolveAll(),
                        n.Resolve("ladderRope")!.ResolveAll(),
                        n.Resolve("life")!.ResolveAll(),
                        n.Resolve("info")!.ResolveAll()
                    )));
            }));

        return _manager.Count;
    }
}
