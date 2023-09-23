using Edelstein.Common.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class ModifiedQuestTimeManagerInit : IPipelinePlug<StageStart>
{
    private readonly IModifiedQuestTimeManager _manager;
    private readonly ITemplateManager<ModifiedQuestTimeTemplate> _templates;
    
    public ModifiedQuestTimeManagerInit(IModifiedQuestTimeManager manager, ITemplateManager<ModifiedQuestTimeTemplate> templates)
    {
        _manager = manager;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, StageStart message) 
        => await Task.WhenAll((await _templates.RetrieveAll()).Select(_manager.Insert));
}
