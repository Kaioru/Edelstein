using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserAttackPlug : IPipelinePlug<FieldOnPacketUserAttack>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAttack message)
    {
        var operation = (PacketSendOperations)((int)PacketSendOperations.UserMeleeAttack + (int)message.Attack.Type);
        var packet = new PacketWriter(operation);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteByte((byte)(
            message.Attack.DamagePerMob |
            16 *  message.Attack.MobCount
        ));
        packet.WriteByte(message.User.Character.Level);

        if (message.Attack.SkillID > 0)
        {
            packet.WriteByte(1);
            packet.WriteInt(message.Attack.SkillID);
        }
        else packet.WriteByte(0);

        packet.WriteByte((byte)(
            0x1 * Convert.ToByte(message.Attack.IsFinalAfterSlashBlast) |
            0x2 * Convert.ToByte(message.Attack.IsSoulArrow) |
            0x8 * Convert.ToByte(message.Attack.IsShadowPartner) |
            0x20 * Convert.ToByte(message.Attack.IsSerialAttack) |
            0x40 * Convert.ToByte(message.Attack.IsSpiritJavelin)
        ));
        packet.WriteShort((short)(
            message.Attack.Action & 0x7FFF |
            Convert.ToByte(message.Attack.IsLeft) << 15
        ));

        if (message.Attack.Action <= 0x110)
        {
            packet.WriteByte(0); // Mastery
            packet.WriteByte(0); // v82
            packet.WriteInt(0); // MovingShoot
            
            foreach (var entry in message.Attack.Entries)
            {
                packet.WriteInt(entry.MobID);
                packet.WriteByte(entry.HitAction);
                
                foreach (var damage in entry.Damage)
                {
                    packet.WriteBool(false); // Critical
                    packet.WriteInt(damage);
                }
            }
        }

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);
    }
}
