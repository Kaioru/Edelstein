using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Common.Gameplay.Game.Objects.Mob.Stats.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats.Modify;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMob : 
    AbstractFieldControllable<IFieldMobMovePath, IFieldMobMoveAction>, 
    IFieldMob, 
    IPacketWritable, 
    ITickable
{
    private readonly SemaphoreSlim _lock;

    public FieldMob(
        IMobTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        IFieldFoothold? footholdHome = null,
        bool isFacingLeft = true
    ) : base(new FieldMobMoveAction(template.MoveAbility, isFacingLeft), position, foothold)
    {
        _lock = new SemaphoreSlim(1, 1);
        LastUpdateBurned = DateTime.UtcNow;
        
        Template = template;
        
        FootholdHome = footholdHome;

        Stats = new FieldMobStats();
        TemporaryStats = new MobTemporaryStats();
        HP = template.MaxHP;
        MP = template.MaxMP;
        
        UpdateStats().Wait();
    }

    public override FieldObjectType Type => FieldObjectType.Mob;

    public IMobTemplate Template { get; }
    
    public IFieldFoothold? FootholdHome { get; }

    public IFieldMobStats Stats { get; }
    public IMobTemporaryStats TemporaryStats { get; }
    public int HP { get; private set; }
    public int MP { get; private set; }
    
    private DateTime LastUpdateBurned { get; set; }
    
    public async Task Damage(int damage, IFieldUser? attacker = null)
    {
        await _lock.WaitAsync();
        
        try
        {
            if (Field == null) return;
            if (attacker != null) await Control(attacker);

            HP -= damage;

            if (attacker != null && damage > 0)
            {
                var indicatorPacket = new PacketWriter(PacketSendOperations.MobHPIndicator);
                var indicator = HP / (float)Template.MaxHP * 100f;

                indicator = Math.Min(100, indicator);
                indicator = Math.Max(0, indicator);

                indicatorPacket.WriteInt(ObjectID ?? 0);
                indicatorPacket.WriteByte((byte)indicator);

                await attacker.Dispatch(indicatorPacket.Build());
            }

            if (HP <= 0)
            {
                await Field.Leave(this, () => GetLeaveFieldPacket(FieldMobLeaveType.Etc));

                if (attacker != null) 
                    _ = attacker.StageUser.Context.Managers.Quest.UpdateMobKill(attacker, Template.ID);
            }
        }
        finally
        {
            _lock.Release();
        }
    }
    public async Task ModifyTemporaryStats(Action<IModifyMobTemporaryStatContext> action)
    {
        var context = new ModifyMobTemporaryStatsContext(TemporaryStats);

        action.Invoke(context);
        await UpdateStats();

        if (context.HistoryReset.Records.Any() || context.HistoryReset.BurnedInfo.Any())
        {
            var resetPacket = new PacketWriter(PacketSendOperations.MobStatReset);

            resetPacket.WriteInt(ObjectID ?? 0);
            resetPacket.WriteMobTemporaryStatsFlag(context.HistoryReset);

            if (context.HistoryReset.BurnedInfo.Count > 0)
            {
                resetPacket.WriteInt(context.HistoryReset.BurnedInfo.Count);
                foreach (var burned in context.HistoryReset.BurnedInfo)
                {
                    resetPacket.WriteInt(burned.CharacterID);
                    resetPacket.WriteInt(burned.SkillID);
                }
            }
            
            resetPacket.WriteByte(0); // CalcDamageStatIndex
            resetPacket.WriteBool(false); // Movement stuff

            if (FieldSplit != null) 
                await FieldSplit.Dispatch(resetPacket.Build());
        }

        if (context.HistorySet.Records.Any() || context.HistorySet.BurnedInfo.Any())
        {
            var setPacket = new PacketWriter(PacketSendOperations.MobStatSet);

            setPacket.WriteInt(ObjectID ?? 0);
            setPacket.WriteMobTemporaryStats(context.HistorySet);
            setPacket.WriteShort(0); // tDelay
            setPacket.WriteByte(0); // CalcDamageStatIndex
            setPacket.WriteBool(false); // Movement stuff

            if (FieldSplit != null) 
                await FieldSplit.Dispatch(setPacket.Build());
        }
    }

    public override IPacket GetEnterFieldPacket() => GetEnterFieldPacket(FieldMobAppearType.Normal);

    public override IPacket GetLeaveFieldPacket() => GetLeaveFieldPacket(FieldMobLeaveType.None);
    
    public void WriteTo(IPacketWriter writer) => WriteTo(writer, FieldMobAppearType.Normal);

    public IPacket GetEnterFieldPacket(FieldMobAppearType appear, int? appearOption = null)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobEnterField);

        packet.WriteInt(ObjectID ?? 0);
        WriteTo(packet, appear, appearOption);
        return packet.Build();
    }
    
    public IPacket GetLeaveFieldPacket(FieldMobLeaveType leaveType)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobLeaveField);

        packet.WriteInt(ObjectID ?? 0);
        packet.WriteByte((byte)leaveType);
        return packet.Build();
    }
    
    private void WriteTo(IPacketWriter writer, FieldMobAppearType appear, int? appearOption = null)
    {
        writer.WriteByte(1); // CalcDamageStatIndex
        writer.WriteInt(Template.ID);

        writer.WriteMobTemporaryStats(TemporaryStats);

        writer.WritePoint2D(Position);
        writer.WriteByte(Action.Raw);
        writer.WriteShort((short)(Foothold?.ID ?? 0));
        writer.WriteShort((short)(FootholdHome?.ID ?? 0));

        writer.WriteByte((byte)appear);
        if (appear is FieldMobAppearType.Revived or >= 0)
            writer.WriteInt(appearOption ?? 0);

        writer.WriteByte(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
    }

    protected override IPacket GetMovePacket(IFieldMobMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobMove);

        packet.WriteInt(ObjectID!.Value);
        packet.Write(ctx);

        return packet.Build();
    }

    protected override IPacket GetControlPacket(IFieldObjectController? controller = null)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobChangeController);

        packet.WriteBool(controller != null);
        packet.WriteInt(ObjectID ?? 0);

        if (controller != null)
            WriteTo(packet, FieldMobAppearType.Regen);
        return packet.Build();
    }
    
    private Task UpdateStats() 
        => Stats.Apply(this);

    public async Task OnTick(DateTime now)
    {
        if (TemporaryStats.BurnedInfo.Count > 0)
        {
            foreach (var burned in TemporaryStats.BurnedInfo)
            {
                var attacker = Field?
                    .GetPool(FieldObjectType.User)?
                    .GetObject(burned.CharacterID) 
                    as IFieldUser;
                var times = (int)((now - LastUpdateBurned).TotalMilliseconds / burned.Interval.TotalMilliseconds);
                var damage = times * burned.Damage;

                // fixedDamage check
                // onlyNormalAttack check

                _ = Damage(Math.Min(HP - 1, damage), attacker);
            }
        }

        LastUpdateBurned = now;
        
        var expiredStats = TemporaryStats.Records
            .Where(kv => kv.Value.DateExpire < now)
            .ToImmutableList();
        var expiredBurned = TemporaryStats.BurnedInfo
            .Where(b => b.DateExpire < now)
            .ToImmutableList();

        if (expiredStats.Count > 0)
        {
            await ModifyTemporaryStats(s =>
            {
                foreach (var kv in expiredStats)
                    s.ResetByType(kv.Key);
            });
        }

        if (expiredBurned.Count > 0)
        {
            await ModifyTemporaryStats(s =>
            {
                foreach (var burned in expiredBurned)
                    s.ResetBurnedInfo(burned);
            });
        }
    }
}
