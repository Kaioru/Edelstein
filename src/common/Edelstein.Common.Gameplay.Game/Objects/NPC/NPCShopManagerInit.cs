using Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public class NPCShopManagerInit : IPipelinePlug<StageStart>
{
    private readonly INPCShopManager _manager;
    private readonly ITemplateManager<NPCShopTemplate> _templates;
    
    public NPCShopManagerInit(INPCShopManager manager, ITemplateManager<NPCShopTemplate> templates)
    {
        _manager = manager;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, StageStart message) 
        => await Task.WhenAll((await _templates.RetrieveAll()).Select(_manager.Insert));
}
