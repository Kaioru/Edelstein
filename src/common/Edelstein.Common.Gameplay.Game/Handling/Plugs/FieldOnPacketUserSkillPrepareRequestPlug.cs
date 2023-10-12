using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSkillPrepareRequestPlug : IPipelinePlug<FieldOnPacketUserSkillPrepareRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSkillPrepareRequest message)
    {
        var p = new PacketWriter(PacketSendOperations.UserSkillPrepare);

        p.WriteInt(message.User.Character.ID);
        p.WriteInt(message.SkillID);
        p.WriteByte(message.SkillLevel);
        p.WriteShort(message.MoveAction);
        p.WriteByte(message.ActionSpeed);

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(p.Build(), message.User);
    }
}
