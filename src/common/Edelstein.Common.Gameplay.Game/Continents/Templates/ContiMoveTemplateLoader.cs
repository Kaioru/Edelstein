using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Continents.Templates;

public class ContiMoveTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IContiMoveTemplate> _manager;

    public ContiMoveTemplateLoader(IDataNamespace data, ITemplateManager<IContiMoveTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.ResolvePath("Server/Continent.img");

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
                        n.ResolvePath("field")!.ResolveAll(),
                        n.ResolvePath("scheduler")!.ResolveAll(),
                        n.ResolvePath("genMob")?.ResolveAll(),
                        n.ResolvePath("time")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
