using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserAttackPlug : IPipelinePlug<FieldOnPacketUserAttack>
{
    private readonly ISkillManager _skillManager;
    
    public FieldOnPacketUserAttackPlug(ISkillManager skillManager) 
        => _skillManager = skillManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAttack message)
    {
        var mobs = message.Attack.Entries.ToImmutableDictionary(
            kv => kv.MobID,
            kv => message.User.Field?.GetObject<IFieldMob>(kv.MobID)
        );
        var skillID = message.Attack.SkillID;
        var skillLevel = skillID > 0 ? message.User.Stats.SkillLevels[skillID] : 0;
        var operation = (PacketSendOperations)((int)PacketSendOperations.UserMeleeAttack + (int)message.Attack.Type);
        var packet = new PacketWriter(operation);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteByte((byte)(
            0x1 * message.Attack.DamagePerMob |
            0x10 * message.Attack.MobCount
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
            0x40 * Convert.ToByte(message.Attack.IsSpiritJavelin) |
            0x80 * Convert.ToByte(message.Attack.IsSpark)
        ));
        packet.WriteShort((short)(
            message.Attack.Action & 0x7FFF |
            Convert.ToByte(message.Attack.IsLeft) << 15
        ));

        packet.WriteByte((byte)message.Attack.SpeedDegree);
        packet.WriteByte((byte)message.User.Stats.Mastery);
        packet.WriteInt(0); // BulletCashItemID

        var count = 0;

        foreach (var entry in message.Attack.Entries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            var attack = new UserAttack(
                skillID,
                skillLevel,
                message.Attack.Keydown,
                message.Attack.IsFinalAfterSlashBlast,
                message.Attack.IsSoulArrow,
                message.Attack.IsShadowPartner,
                message.Attack.IsSerialAttack,
                message.Attack.IsSpiritJavelin,
                message.Attack.IsSpark
            );
            var damageSrv = await message.User.Damage.AdjustDamageDecRate(
                attack,
                count,
                message.Attack.Type == AttackType.Magic 
                    ? await message.User.Damage.CalculateMDamage(message.User.Character, message.User.Stats, mob, mob.Stats, attack)
                    : await message.User.Damage.CalculatePDamage(message.User.Character, message.User.Stats, mob, mob.Stats, attack)
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

            count++;
        }

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);

        if (!await _skillManager.ProcessUserAttack(message.User, message.Attack))
            return;

        foreach (var entry in message.Attack.Entries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            await _skillManager.ProcessUserAttackMob(message.User, mob, message.Attack, entry);
        }
    }
}
