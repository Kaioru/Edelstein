using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class MobQuestCacheManagerInit : IPipelinePlug<StageStart>
{
    private readonly IMobQuestCacheManager _manager;
    private readonly ITemplateManager<IQuestTemplate> _templates;
    
    public MobQuestCacheManagerInit(IMobQuestCacheManager manager, ITemplateManager<IQuestTemplate> templates)
    {
        _manager = manager;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        var quests = await _templates.RetrieveAll();
        foreach (var quest in quests)
        {
            if (quest.CheckEnd.CheckMob == null) continue;
            foreach (var mob in quest.CheckEnd.CheckMob)
                (await _manager.Retrieve(mob.MobID))?.Quests.Add(quest.ID);
        }
    }
}
