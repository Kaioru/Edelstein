using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserSortItemRequest : AbstractUserOnPacketInField<UserSortItemRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserSortItemRequest> message)
    {
        await message.User.ModifyInventory(i => i[message.Packet.Type]?.Sort(), true);
        await message.User.Dispatch(new SortItemResult
        {
            Type = message.Packet.Type
        });
    }
}
