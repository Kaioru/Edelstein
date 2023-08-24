using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserSortItemRequestPlug : IPipelinePlug<FieldOnPacketUserSortItemRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSortItemRequest message)
    {
        using var packet = new PacketWriter(PacketSendOperations.SortItemResult);
        
        packet.WriteBool(false);
        packet.WriteByte((byte)message.Type);
        
        await message.User.ModifyInventory(i => i[message.Type]?.Sort(), true);
        await message.User.Dispatch(packet.Build());
    }
}
