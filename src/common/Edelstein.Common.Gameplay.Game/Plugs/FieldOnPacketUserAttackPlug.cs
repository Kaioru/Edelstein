using System.Collections.Immutable;
using System.ComponentModel;
using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserAttackPlug : IPipelinePlug<FieldOnPacketUserAttack>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAttack message)
    {
        var mobs = message.Attack.Entries.ToImmutableDictionary(
            kv => kv.MobID,
            kv => message.User.Field?.GetObject<IFieldMob>(kv.MobID)
        );
        var skillID = message.Attack.SkillID;
        var skillLevel = skillID > 0 ? message.User.Character.Skills[skillID]?.Level ?? 0 : 0;
        var operation = (PacketSendOperations)((int)PacketSendOperations.UserMeleeAttack + (int)message.Attack.Type);
        var packet = new PacketWriter(operation);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteByte((byte)(
            0x1 * message.Attack.DamagePerMob |
            0x10 *  message.Attack.MobCount
        ));
        packet.WriteByte(message.User.Character.Level);

        if (message.Attack.SkillID > 0)
        {
            packet.WriteByte((byte)(message.User.Character.Skills[message.Attack.SkillID]?.Level ?? 1));
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

        packet.WriteByte((byte)message.Attack.SpeedDegree);
        packet.WriteByte((byte)message.User.Stats.Mastery);
        packet.WriteInt(0); // BulletCashItemID
        
        foreach (var entry in message.Attack.Entries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            var damageSrv = await message.User.Damage.CalculatePDamage(
                message.User.Stats,
                mob.Stats,
                new UserAttack(skillID, skillLevel, message.Attack.Keydown)
            );

            for (var i = 0; i < message.Attack.DamagePerMob; i++)
                Console.WriteLine($"Client: {entry.Damage[i]}, Server: {damageSrv[i].Damage}, Critical: {damageSrv[i].IsCritical}");

            packet.WriteInt(entry.MobID);
            packet.WriteByte(entry.HitAction);

            for (var i = 0; i < message.Attack.DamagePerMob; i++)
            {
                packet.WriteBool(damageSrv[i].IsCritical);
                packet.WriteInt(entry.Damage[i]);
            }
        }
        
        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);
        
        foreach (var entry in message.Attack.Entries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            await mob.Damage(entry.Damage.Sum(), message.User);
        }
    }
}
