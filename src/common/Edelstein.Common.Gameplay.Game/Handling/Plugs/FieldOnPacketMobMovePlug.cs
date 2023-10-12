using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketMobMovePlug : IPipelinePlug<FieldOnPacketMobMove>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketMobMove message)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobCtrlAck);

        packet.WriteInt(message.Mob.ObjectID ?? 0);
        packet.WriteShort(message.Path.MobCtrlSN);
        packet.WriteBool(message.Path.NextAttackPossible);
        packet.WriteShort(0); // nMP
        packet.WriteByte(0); // SkillCommand
        packet.WriteByte(0); // SLV
        
        await message.User.Dispatch(packet.Build());
        await message.Mob.Move(message.Path, message.User);
    }
}
