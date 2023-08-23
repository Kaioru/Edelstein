using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Continents.Templates;

public class ContiMoveTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IContiMoveTemplate> _manager;

    public ContiMoveTemplateLoader(IDataManager data, ITemplateManager<IContiMoveTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("Server/Continent.img");

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderLazy<IContiMoveTemplate>(
                    id,
                    () => new ContiMoveTemplate(
                        id,
                        n.ResolveAll(),
                        n.Resolve("field")!.ResolveAll(),
                        n.Resolve("scheduler")!.ResolveAll(),
                        n.Resolve("genMob")?.ResolveAll(),
                        n.Resolve("time")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
