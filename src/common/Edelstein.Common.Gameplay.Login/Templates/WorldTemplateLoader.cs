using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Login.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Login.Templates;

public class WorldTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<IWorldTemplate> _manager;

    public WorldTemplateLoader(IDataManager data, ITemplateManager<IWorldTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("Server/World.img");

        if (directory == null) return 0;
        foreach (var node in directory)
        {
            var id = Convert.ToInt32(node.Name.Split(".")[0]);

            await _manager.Insert(new TemplateProviderLazy<IWorldTemplate>(
                id,
                () => new WorldTemplate(id, node)
            ));
        }

        _manager.Freeze();
        return _manager.Count;
    }
}
