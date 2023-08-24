using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserGatherItemRequestPlug : IPipelinePlug<FieldOnPacketUserGatherItemRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserGatherItemRequest message)
    {
        using var packet = new PacketWriter(PacketSendOperations.GatherItemResult);
        
        packet.WriteBool(false);
        packet.WriteByte((byte)message.Type);
        
        await message.User.ModifyInventory(i => i[message.Type]?.Gather(), true);
        await message.User.Dispatch(packet.Build());
    }
}
