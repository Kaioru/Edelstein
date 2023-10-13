using System.Collections.Immutable;
using Edelstein.Common.Constants;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

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
        
        if (message.Attack.AttackSpeed != message.User.Stats.AttackSpeed)
            _logger.LogInformation(
            "{Character} triggered a {Type} attack speed calculation mismatch with skill id: {Skill} (Client: {SpeedClient}, Server: {SpeedServer})",
                message.User.Character.Name,
                message.Attack.Type,
                message.Attack.SkillID,
                message.Attack.AttackSpeed,
                message.User.Stats.AttackSpeed
            );
        
        using var packet = new PacketWriter(operation);

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
        
        if (message.Attack is
            {
                Type: AttackType.Melee,
                SkillID:
                Skill.BmageTripleBlow or
                Skill.BmageQuadBlow or
                Skill.BmageFinishBlow or 
                Skill.BmageFinishAttack or 
                Skill.BmageFinishAttack1 or 
                Skill.BmageFinishAttack2 or 
                Skill.BmageFinishAttack3 or 
                Skill.BmageFinishAttack4 or 
                Skill.BmageFinishAttack5
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
            packet.WriteByte((byte)message.User.Stats.SkillLevels[message.Attack.SkillID]);
            packet.WriteInt(message.Attack.SkillID);
        }
        else packet.WriteByte(0);

        packet.WriteByte(message.Attack.Option);

        packet.WriteShort(message.Attack.AttackActionAndDir);
        packet.WriteByte(message.Attack.AttackSpeed);

        packet.WriteByte((byte)message.User.Stats.Mastery);
        packet.WriteInt(0); // BulletCashItemID

        var mobOrder = 0;

        foreach (var entry in message.Attack.MobEntries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            
            if (mob == null)
            {
                message.User.Damage.Skip();
                continue;
            }
            
            var damage = await (isPDamage
                ? message.User.Damage.CalculatePDamage(message.User.Character, message.User.Stats, mob, mob.Stats, message.Attack, entry)
                : message.User.Damage.CalculateMDamage(message.User.Character, message.User.Stats, mob, mob.Stats, message.Attack, entry));
            var adjustedDamage = await message.User.Damage.CalculateAdjustedDamage(
                message.User.Character, 
                message.User.Stats, 
                mob, 
                mob.Stats, 
                message.Attack, 
                damage, 
                message.Attack.MobCount,
                mobOrder
            );

            packet.WriteInt(entry.MobID);
            packet.WriteByte(entry.ActionHit);

            var isMismatch = false;

            if (entry.Damage.Length != adjustedDamage.Length)
            {
                isMismatch = true;
                _logger.LogInformation(
                    "{Character} triggered a {Type} attack count mismatch with skill id: {Skill} (Client: {Count}, Server: {CountServer})",
                    message.User.Character.Name,
                    message.Attack.Type,
                    message.Attack.SkillID,
                    entry.Damage.Length,
                    adjustedDamage.Length
                );
            }

            for (var i = 0; i < message.Attack.DamagePerMob; i++)
            {
                if (!isMismatch && entry.Damage[i] != adjustedDamage[i])
                    _logger.LogInformation(
                        "{Character} triggered a {Type} attack damage calculation mismatch with skill id: {Skill} (Index: {Index}, Client: {Damage}, Server: {DamageServer}, Critical: {IsCritical})",
                        message.User.Character.Name,
                        message.Attack.Type,
                        message.Attack.SkillID,
                        i,
                        entry.Damage[i],
                        adjustedDamage[i],
                        damage[i].IsCritical
                    );
                
                packet.WriteBool(!isMismatch && damage[i].IsCritical);
                packet.WriteInt(entry.Damage[i]);
            }

            mobOrder++;
        }

        if (message.Attack.Type == AttackType.Shoot)
            packet.WritePoint2D(new Point2D());

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
