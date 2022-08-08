using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserSortItemRequestPlug : IPipelinePlug<IUserSortItemRequest>
{
    public async Task Handle(IPipelineContext ctx, IUserSortItemRequest message)
    {
        var response = new PacketWriter(PacketSendOperations.SortItemResult);

        response.WriteBool(false);
        response.WriteByte((byte)message.Type);

        await message.User.ModifyInventory(
            i => i[message.Type]?.Sort(),
            true
        );
        await message.User.Dispatch(response);
    }
}
