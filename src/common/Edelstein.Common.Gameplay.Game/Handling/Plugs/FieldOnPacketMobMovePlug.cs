using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketMobMovePlug : IPipelinePlug<FieldOnPacketMobMove>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketMobMove message)
    {
        using var p = new PacketWriter(PacketSendOperations.MobCtrlAck);

        p.WriteInt(message.Mob.ObjectID ?? 0);
        p.WriteShort(message.Path.MobCtrlSN);
        p.WriteBool(message.Path.NextAttackPossible);
        p.WriteShort(0); // nMP
        p.WriteByte(0); // SkillCommand
        p.WriteByte(0); // SLV
        
        await message.User.Dispatch(p.Build());
        await message.Mob.Move(message.Path, message.User);
    }
}
