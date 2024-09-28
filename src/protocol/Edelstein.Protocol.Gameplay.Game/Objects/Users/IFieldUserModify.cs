using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUserModify
{
    bool IsRequireUpdate { get; }
    bool IsRequireUpdateAvatar { get; }
    
    Task Stats(Action<IModifyStatContext>? action = null, bool exclRequest = false);
    Task TemporaryStats(Action<IModifyTemporaryStatContext> action, short delay = 0);

    Task Inventory(Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false);
}
