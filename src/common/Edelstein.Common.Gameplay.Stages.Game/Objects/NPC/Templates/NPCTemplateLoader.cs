using Edelstein.Common.Util.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;

public class NPCTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<INPCTemplate> _manager;

    public NPCTemplateLoader(IDataManager data, ITemplateManager<INPCTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.Resolve("Npc")?.ResolveAll();

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await _manager.Insert(new TemplateProviderLazy<INPCTemplate>(
                    id,
                    () => new NPCTemplate(
                        id,
                        n.ResolveAll(),
                        n.Resolve("info")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
