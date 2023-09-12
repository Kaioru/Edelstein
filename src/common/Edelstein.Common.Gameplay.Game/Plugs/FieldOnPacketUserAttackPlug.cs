using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Combat.Damage;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserAttackPlug : IPipelinePlug<FieldOnPacketUserAttack>
{
    private readonly ILogger _logger;
    private readonly ISkillManager _skillManager;
    
    public FieldOnPacketUserAttackPlug(ILogger<FieldOnPacketUserAttackPlug> logger, ISkillManager skillManager)
    {
        _logger = logger;
        _skillManager = skillManager;
    }
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAttack message)
    {
        var mobs = message.Attack.MobEntries.ToImmutableDictionary(
            kv => kv.MobID,
            kv => message.User.Field?.GetObject<IFieldMob>(kv.MobID)
        );
        var skillID = message.Attack.SkillID;
        var skillLevel = skillID > 0 ? message.User.Stats.SkillLevels[skillID] : 0;
        var isPDamage = message.Attack.Type != AttackType.Magic;
        var operation = (PacketSendOperations)((int)PacketSendOperations.UserMeleeAttack + (int)message.Attack.Type);
        var packet = new PacketWriter(operation);

        if (message.Attack is
            {
                Type: AttackType.Body,
                SkillID:
                Skill.BmageCyclone or
                Skill.Mage1TeleportMastery or
                Skill.Mage2TeleportMastery or
                Skill.PriestTeleportMastery or
                Skill.BmageTeleportMastery
            }
           ) isPDamage = false;

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

        packet.WriteByte(message.Attack.Option);

        packet.WriteShort(message.Attack.AttackActionAndDir);
        packet.WriteByte(message.Attack.AttackSpeed);

        packet.WriteByte((byte)message.User.Stats.Mastery);
        packet.WriteInt(0); // BulletCashItemID

        var count = 0;

        foreach (var entry in message.Attack.MobEntries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            var damage = await (isPDamage
                ? message.User.Damage.CalculatePDamage(message.User.Character, message.User.Stats, mob, mob.Stats, message.Attack, entry)
                : message.User.Damage.CalculateMDamage(message.User.Character, message.User.Stats, mob, mob.Stats, message.Attack, entry));
            var adjustedDamage = await message.User.Damage.CalculateAdjustedDamage(message.User.Character, message.User.Stats, message.Attack, damage, count);

            packet.WriteInt(entry.MobID);
            packet.WriteByte(entry.ActionHit);

            for (var i = 0; i < message.Attack.DamagePerMob; i++)
            {
                packet.WriteBool(false);
                packet.WriteInt(entry.Damage[i]);

                if (entry.Damage[i] != adjustedDamage[i])
                    _logger.LogInformation(
                        "{Character} triggered a {Type} attack damage calculation mismatch with skill id: {Skill} (Client: {Damage}, Server: {DamageServer}, Critical: {IsCritical})",
                        message.User.Character.Name,
                        message.Attack.Type,
                        message.Attack.SkillID,
                        entry.Damage[i],
                        adjustedDamage[i],
                        damage[i].IsCritical
                    );
                packet.WriteBool(damage[i].IsCritical);
                packet.WriteInt(entry.Damage[i]);
            }

            count++;
        }

        if (SkillConstants.IsKeydownSkill(message.Attack.SkillID))
            packet.WriteInt(message.Attack.Keydown);

        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);

        if (!await _skillManager.Check(message.User, message.Attack.SkillID))
            return;

        await _skillManager.HandleAttack(message.User, message.Attack.SkillID, message.Attack.MobEntries.Length > 0);

        foreach (var entry in message.Attack.MobEntries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            await _skillManager.HandleAttackMob(message.User, mob, message.Attack.SkillID, entry.Damage.Sum());
        }
    }
}
