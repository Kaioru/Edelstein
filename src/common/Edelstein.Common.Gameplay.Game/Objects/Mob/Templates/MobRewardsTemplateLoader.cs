﻿using Duey.Abstractions;
using Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobRewardsTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<MobRewardsTemplate> _manager;
    
    public MobRewardsTemplateLoader(IDataNamespace data, ITemplateManager<MobRewardsTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("Server/Reward.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<MobRewardsTemplate>(
                    id,
                    new MobRewardsTemplate(id, n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
