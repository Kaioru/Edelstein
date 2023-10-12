using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserThrowGrenadePlug : IPipelinePlug<FieldOnPacketUserThrowGrenade>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserThrowGrenade message)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserThrowGrenade);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteInt(message.Position.X);
        packet.WriteInt(message.Position.Y);
        packet.WriteInt(message.Keydown);
        packet.WriteInt(message.SkillID);
        packet.WriteInt(message.SkillLevel);

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);
    }
}
