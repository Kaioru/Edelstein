using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserChangeSlotPositionRequest : AbstractUserOnPacketInFieldPipe<UserChangeSlotPositionRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserChangeSlotPositionRequest> message)
    {
        if (message.Packet.NewPos == 0)
        {
            await message.User.ModifyInventory(exclRequest: true);
            return;
        }
        
        await message.User.ModifyInventory(
            i => i[message.Packet.Type]?.MoveSlot(message.Packet.OldPos, message.Packet.NewPos),
            true
        );
    }
}
