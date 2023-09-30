using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserThrowGrenadePlug : IPipelinePlug<FieldOnPacketUserThrowGrenade>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserThrowGrenade message)
    {
        var p = new PacketWriter(PacketSendOperations.UserThrowGrenade);

        p.WriteInt(message.User.Character.ID);
        p.WriteInt(message.Position.X);
        p.WriteInt(message.Position.Y);
        p.WriteInt(message.Keydown);
        p.WriteInt(message.SkillID);
        p.WriteInt(message.SkillLevel);

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(p.Build(), message.User);
    }
}
