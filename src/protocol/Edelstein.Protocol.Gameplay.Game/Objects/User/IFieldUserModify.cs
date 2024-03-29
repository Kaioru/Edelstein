﻿using Edelstein.Protocol.Gameplay.Game.Objects.User.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;

namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUserModify
{
    bool IsRequireUpdate { get; }
    bool IsRequireUpdateAvatar { get; }
    
    Task Stats(Action<IModifyStatContext>? action = null, bool exclRequest = false);
    Task Stats(IModifyStatContext context, bool exclRequest = false);

    Task StatsForced(Action<IModifyStatForcedContext>? action = null);
    Task StatsForced(IModifyStatForcedContext context);
    
    Task Inventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
    Task Inventory(IModifyInventoryGroupContext context, bool exclRequest = false);
    
    Task Skills(Action<IModifySkillContext>? action = null, bool exclRequest = false);
    Task Skills(IModifySkillContext context, bool exclRequest = false);
    
    Task TemporaryStats(Action<IModifyTemporaryStatContext>? action = null, bool exclRequest = false);
    Task TemporaryStats(IModifyTemporaryStatContext context, bool exclRequest = false);
}
