using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Modifiers;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public static class FieldUserExtensions
{
    public static Task ModifyStats(this IFieldUser user, Action<IModifyStatContext>? action = null, bool exclRequest = false)
        => user.Modify(m => m.Stats(action, exclRequest));
    
    public static Task ModifyInventory(this IFieldUser user, Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false)
        => user.Modify(m => m.Inventory(action, exclRequest));
}
