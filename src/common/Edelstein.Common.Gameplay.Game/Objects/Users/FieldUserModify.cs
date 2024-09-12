using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public class FieldUserModify(
    IFieldUser user
) : IFieldUserModify
{
    public bool IsRequireUpdate { get; private set; }
    public bool IsRequireUpdateAvatar { get; private set; }
    
    public Task Inventory(Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryContextGroup(
            user.Character.Inventories,
            user.System.Context.Templates.Items
        );
        
        action?.Invoke(context);
        
        if (context.IsUpdated)
            IsRequireUpdate = true;
        if (context.IsUpdatedAvatar)
            IsRequireUpdateAvatar = true;

        return user.Dispatch(new InventoryOperation
        {
            ExclRequest = exclRequest,
            Operations = context.ToStructured(),
            SN = (byte)Random.Shared.Next(byte.MinValue, byte.MaxValue)
        });
    }
}
