using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketSummonedAttackPlug : IPipelinePlug<FieldOnPacketSummonedAttack>
{
    private readonly ILogger _logger;
    private readonly ISkillManager _skillManager;
    
    public FieldOnPacketSummonedAttackPlug(ILogger<FieldOnPacketSummonedAttackPlug> logger, ISkillManager skillManager)
    {
        _logger = logger;
        _skillManager = skillManager;
    }

    public async Task Handle(IPipelineContext ctx, FieldOnPacketSummonedAttack message)
    {
        var mobs = message.Attack.MobEntries.ToImmutableDictionary(
            kv => kv.MobID,
            kv => message.User.Field?.GetObject<IFieldMob>(kv.MobID)
        );
        var packet = new PacketWriter(PacketSendOperations.SummonedAttack);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteInt(message.Summoned.ObjectID ?? 0);
        packet.WriteByte(message.User.Character.Level);
        
        packet.WriteByte(message.Attack.AttackActionAndDir);
        
        packet.WriteByte(message.Attack.MobCount);
        
        foreach (var entry in message.Attack.MobEntries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            var damage = await message.User.Damage.CalculateMDamage(
                message.User.Character,
                message.User.Stats,
                mob,
                mob.Stats,
                message.Summoned
            );

            if (entry.Damage[0] != damage)
                _logger.LogInformation(
                    "{Character} triggered a summoned attack damage calculation mismatch with summoned skill id: {Skill} (Client: {Damage}, Server: {DamageServer})",
                    message.User.Character.Name,
                    message.Summoned.SkillID,
                    entry.Damage[0],
                    damage
                );
            
            packet.WriteInt(entry.MobID);
            packet.WriteBool(false);
            packet.WriteInt(entry.Damage[0]);
        }

        packet.WriteByte(0);
        
        if (message.User.FieldSplit != null)
            await message.User.FieldSplit.Dispatch(packet.Build(), message.User);

        if (!await _skillManager.Check(message.User, message.Summoned.SkillID))
            return;

        await _skillManager.HandleAttack(message.User, message.Summoned.SkillID, message.Attack.MobEntries.Length > 0);

        foreach (var entry in message.Attack.MobEntries)
        {
            var mob = mobs.TryGetValue(entry.MobID, out var e) ? e : null;
            if (mob == null) continue;
            await _skillManager.HandleAttackMob(message.User, mob, message.Summoned.SkillID, entry.Damage.Sum());
        }
    }
}
