using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserChangeSlotPositionRequest : AbstractUserOnPacketInField<UserChangeSlotPositionRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserChangeSlotPositionRequest> message)
    {
        if (message.Packet.NewPos == 0)
        {
            await message.User.ModifyInventory(exclRequest: true);
            return;
        }

        var inventory = message.User.Character.Inventories[message.Packet.Type];
        var item = inventory?[message.Packet.OldPos];

        if (message.Packet.NewPos < 0 && item is ItemSlotEquip)
        {
            var bodyParts = item.TemplateID.GetBodyParts();
            var bodyPart = (BodyPart)Math.Abs(message.Packet.NewPos);
            
            if (bodyParts.All(bp => bp != bodyPart)) return;
        }
        
        await message.User.ModifyInventory(
            i => i[message.Packet.Type]?.MoveSlot(message.Packet.OldPos, message.Packet.NewPos),
            true
        );
    }
}
