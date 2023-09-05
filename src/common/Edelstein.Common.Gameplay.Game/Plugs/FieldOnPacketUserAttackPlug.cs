using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserAttackPlug : IPipelinePlug<FieldOnPacketUserAttack>
{
    private ITemplateManager<ISkillTemplate> _skillTemplates;
    
    public FieldOnPacketUserAttackPlug(ITemplateManager<ISkillTemplate> skillTemplates) => _skillTemplates = skillTemplates;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserAttack message)
    {
        var random = new Random();
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
                message.User.Character,
                message.User.Stats,
                mob,
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
            
            if (mob.HP <= 0) continue;
            var skill = await _skillTemplates.Retrieve(skillID);
            var level = skill?.Levels[message.User.Character.Skills[skillID]?.Level ?? 0];
            if (skill == null || level == null) continue;
            if (level.Prop > 0 && random.Next(0, 100) > level.Prop) continue;
            var stats = new List<Tuple<MobTemporaryStatType, short>>();
            var expire = DateTime.UtcNow.AddSeconds(level.Time);
            
            switch (skillID)
            {
                case Skill.CrusaderPanic:
                case Skill.SoulmasterPanicSword:
                    // TODO: not working?
                    stats.Add(Tuple.Create(MobTemporaryStatType.Darkness, level.X));
                    break;
                case Skill.CrusaderComa:
                case Skill.SoulmasterComaSword:
                    stats.Add(Tuple.Create(MobTemporaryStatType.Stun, (short)1));
                    break;
                case Skill.CrusaderShout:
                    stats.Add(Tuple.Create(MobTemporaryStatType.Stun, (short)1));
                    break;
                case Skill.HeroMonsterMagnet:
                case Skill.DarkknightMonsterMagnet:
                    stats.Add(Tuple.Create(MobTemporaryStatType.Stun, (short)1));
                    break;
            }

            await mob.ModifyTemporaryStats(s =>
            {
                s.ResetByReason(skillID);
                foreach (var tuple in stats)
                    s.Set(tuple.Item1, tuple.Item2, skillID, expire);
            });
        }

        var character = message.User.Character;
        var comboCounterStat = character.TemporaryStats[TemporaryStatType.ComboCounter];

        if (comboCounterStat != null && skillID is
            Skill.CrusaderPanic or
            Skill.CrusaderComa or
            Skill.SoulmasterPanicSword or
            Skill.SoulmasterComaSword)
        {
            await message.User.ModifyTemporaryStats(s => s.Set(
                TemporaryStatType.ComboCounter,
                1,
                comboCounterStat.Reason,
                comboCounterStat.DateExpire
            ));
        }
        else if (comboCounterStat != null && message.Attack.Entries.Count > 0)
        {
            var comboCounterSkill = await _skillTemplates.Retrieve(comboCounterStat.Reason);
            var comboCounterLevel = comboCounterSkill?.Levels[character.Skills[comboCounterStat.Reason]?.Level ?? 0];
            var comboCounter = comboCounterStat.Value - 1;
            var comboMax = comboCounterLevel?.X ?? 0;

            var advComboCounterSkillID = JobConstants.GetJobRace(character.Job) == 0
                ? Skill.HeroAdvancedCombo
                : Skill.SoulmasterAdvancedCombo;
            var advComboCounterSkill = await _skillTemplates.Retrieve(advComboCounterSkillID);
            var advComboCounterLevel = advComboCounterSkill?.Levels[character.Skills[advComboCounterSkillID]?.Level ?? 0];
            var comboDoubleChance = advComboCounterLevel?.Prop ?? 0;
            
            comboMax = advComboCounterLevel?.X ?? comboMax;
            
            if (comboCounter < comboMax)
                await message.User.ModifyTemporaryStats(s => s.Set(
                    TemporaryStatType.ComboCounter,
                    Math.Min(comboMax + 1, comboCounterStat.Value + (random.Next(0, 100) <= comboDoubleChance ? 2 : 1)),
                    comboCounterStat.Reason,
                    comboCounterStat.DateExpire
                ));
        }
    }
}
