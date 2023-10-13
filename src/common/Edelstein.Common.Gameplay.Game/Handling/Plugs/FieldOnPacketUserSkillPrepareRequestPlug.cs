using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSkillPrepareRequestPlug : IPipelinePlug<FieldOnPacketUserSkillPrepareRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillPrepareRequest message)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserSkillPrepare);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteInt(message.SkillID);
        packet.WriteByte(message.SkillLevel);
        packet.WriteShort(message.MoveAction);
        packet.WriteByte(message.ActionSpeed);

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);
    }
}
