using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities.Inventories.Accessors;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Accessors;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public class FieldUserAccess(
    IFieldUser user
) : IFieldUserAccess
{
    public async Task<T> Inventory<T>(Func<IAccessInventory, Task<T>> action)
        => await action.Invoke(new AccessInventory(
            user.Character.Inventories,
            user.System.Context.Templates.Items
        ));
}
