using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserGatherItemRequest : AbstractUserOnPacketInField<UserGatherItemRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserGatherItemRequest> message)
    {
        await message.User.ModifyInventory(i => i[message.Packet.Type]?.Gather(), true);
        await message.User.Dispatch(new GatherItemResult
        {
            Type = message.Packet.Type
        });
    }
}
