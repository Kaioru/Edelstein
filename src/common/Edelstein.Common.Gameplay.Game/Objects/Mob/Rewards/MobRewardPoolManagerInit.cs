using Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Game.Rewards;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public class MobRewardPoolManagerInit : IPipelinePlug<StageStart>
{
    private readonly ITemplateManager<MobRewardPoolTemplate> _templates;
    private readonly IMobRewardPoolManager _manager;
    
    public MobRewardPoolManagerInit(
        ITemplateManager<MobRewardPoolTemplate> templates, 
        IMobRewardPoolManager manager
    )
    {
        _templates = templates;
        _manager = manager;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        foreach (var rewards in await _templates.RetrieveAll())
        {
            var pool = new RewardPool<IMobReward>(rewards.ID);
            foreach (var item in rewards.Items)
                await pool.Insert(item);
            await _manager.Insert(pool);
        }
    }
}
