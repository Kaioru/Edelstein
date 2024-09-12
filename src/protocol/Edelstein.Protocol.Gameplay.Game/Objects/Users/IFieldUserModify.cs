using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUserModify
{
    bool IsRequireUpdate { get; }
    bool IsRequireUpdateAvatar { get; }
    
    Task Inventory(Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false);
}
