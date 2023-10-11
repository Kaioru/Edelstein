using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public class NPCStringTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<INPCStringTemplate> _manager;

    public NPCStringTemplateLoader(IDataNamespace data, ITemplateManager<INPCStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("String/Npc.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<INPCStringTemplate>(
                    id,
                    new NPCStringTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        return _manager.Count;
    }
}
