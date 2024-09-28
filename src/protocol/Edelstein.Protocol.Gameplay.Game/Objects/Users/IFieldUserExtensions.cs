using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Messages;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public static class IFieldUserExtensions
{
    public static Task Message(this IFieldUser user, string chat)
        => user.Dispatch(new Message
        {
            Type = MessageType.SystemMessage,
            Info = new StructuredMessageInfoSystemMessage
            {
                Chat = new LPString(chat)
            }
        });
    
    public static Task ModifyStats(this IFieldUser user, Action<IModifyStatContext>? action = null, bool exclRequest = false)
        => user.Modify(m => m.Stats(action, exclRequest));
    
    public static Task ModifyInventory(this IFieldUser user, Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false)
        => user.Modify(m => m.Inventory(action, exclRequest));
}
