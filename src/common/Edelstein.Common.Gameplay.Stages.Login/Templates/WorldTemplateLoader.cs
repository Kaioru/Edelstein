using Edelstein.Common.Util.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Login.Templates;

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
        var count = 0;

        if (directory == null) return count;

        foreach (var node in directory)
        {
            var id = Convert.ToInt32(node.Name.Split(".")[0]);

            await _manager.Insert(new TemplateProvider<IWorldTemplate>(
                id,
                () => new WorldTemplate(id, node)
            ));
            count++;
        }

        return count;
    }
}
