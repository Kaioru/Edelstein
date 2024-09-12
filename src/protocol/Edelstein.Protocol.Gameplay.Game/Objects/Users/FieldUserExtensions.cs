using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public static class FieldUserExtensions
{
    public static Task ModifyInventory(this IFieldUser user, Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false)
        => user.Modify(m => m.Inventory(action, exclRequest));
}
