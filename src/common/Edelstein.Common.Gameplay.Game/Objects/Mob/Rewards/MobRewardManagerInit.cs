using Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public class MobRewardManagerInit : IPipelinePlug<StageStart>
{
    private readonly ITemplateManager<MobRewardsTemplate> _templates;
    private readonly IMobRewardManager _manager;
    
    public MobRewardManagerInit(ITemplateManager<MobRewardsTemplate> templates, IMobRewardManager manager)
    {
        _templates = templates;
        _manager = manager;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        foreach (var rewards in await _templates.RetrieveAll())
        foreach (var item in rewards.Items)
            await _manager.Insert(rewards.ID, item);
    }
}
