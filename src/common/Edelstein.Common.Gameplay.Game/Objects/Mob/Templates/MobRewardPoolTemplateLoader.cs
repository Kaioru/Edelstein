﻿using Duey.Abstractions;
using Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobRewardPoolTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<MobRewardPoolTemplate> _manager;
    
    public MobRewardPoolTemplateLoader(IDataNamespace data, ITemplateManager<MobRewardPoolTemplate> manager)
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
                await _manager.Insert(new TemplateProviderEager<MobRewardPoolTemplate>(
                    id,
                    new MobRewardPoolTemplate(id, n.Cache())
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
