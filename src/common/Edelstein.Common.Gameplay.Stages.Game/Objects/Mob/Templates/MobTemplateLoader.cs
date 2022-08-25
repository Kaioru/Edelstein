using Edelstein.Common.Util.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;

public class MobTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IMobTemplate> _manager;

    public MobTemplateLoader(IDataManager data, ITemplateManager<IMobTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("Mob")?.ResolveAll();

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .Where(n => n.Name.Split(".")[0].All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await _manager.Insert(new TemplateProviderLazy<IMobTemplate>(
                    id,
                    () => new MobTemplate(
                        id,
                        n.ResolveAll(),
                        n.Resolve("info")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
