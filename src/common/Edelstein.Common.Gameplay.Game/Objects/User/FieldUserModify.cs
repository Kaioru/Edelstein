using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Common.Gameplay.Game.Objects.User.Modify;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Common.Gameplay.Models.Characters.Stats;
using Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public class FieldUserModify : IFieldUserModify
{
    private readonly IFieldUser _user;
    
    public FieldUserModify(IFieldUser user) => _user = user;

    public bool IsRequireUpdate { get; private set; } = false;
    public bool IsRequireUpdateAvatar { get; private set; } = false;
    
    public Task Stats(Action<IModifyStatContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyStatContext(_user.Character);
        action?.Invoke(context);
        return Stats(context, exclRequest);
    }
    
    public async Task Stats(IModifyStatContext context, bool exclRequest = false)
    {
        if (context.Flag > 0)
            IsRequireUpdate = true;
        
        if (!_user.IsInstantiated) return;
        
        using var packet = new PacketWriter(PacketSendOperations.StatChanged);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);
        packet.WriteBool(false);
        
        await _user.Dispatch(packet.Build());

        if (context.Flag.HasFlag(ModifyStatType.Level))
            _ = _user.Effect(new LevelUpEffect(), false);
        if (context.Flag.HasFlag(ModifyStatType.Job))
            _ = _user.Effect(new JobChangedEffect(), false);

        if (_user.StageUser.Party != null && (context.Flag.HasFlag(ModifyStatType.Level) || context.Flag.HasFlag(ModifyStatType.Job)))
            _ = _user.StageUser.Context.Services.Party.UpdateLevelOrJob(new PartyUpdateLevelOrJobRequest(
                _user.StageUser.Party.ID,
                _user.Character.ID,
                _user.Character.Level,
                _user.Character.Job));
    }
    
    public Task StatsForced(Action<IModifyStatForcedContext>? action = null)
    {
        var context = new ModifyStatForcedContext(_user.StatsForced);
        action?.Invoke(context);
        return StatsForced(context);
    }
    
    public async Task StatsForced(IModifyStatForcedContext context)
    {
        if (context.Flag > 0 || context.IsReset)
            IsRequireUpdate = true;

        if (context.IsReset)
        {
            using var resetPacket = new PacketWriter(PacketSendOperations.ForcedStatReset);
            await _user.Dispatch(resetPacket.Build());
        }

        if (context.Flag > 0)
        {
            using var setPacket = new PacketWriter(PacketSendOperations.ForcedStatSet);
            
            setPacket.WriteInt((int)context.Flag);
            
            if ((context.Flag & ModifyStatForcedType.STR) != 0) setPacket.WriteShort(context.STR);
            if ((context.Flag & ModifyStatForcedType.DEX) != 0) setPacket.WriteShort(context.DEX);
            if ((context.Flag & ModifyStatForcedType.INT) != 0) setPacket.WriteShort(context.INT);
            if ((context.Flag & ModifyStatForcedType.LUK) != 0) setPacket.WriteShort(context.LUK);
            
            if ((context.Flag & ModifyStatForcedType.PAD) != 0) setPacket.WriteShort(context.PAD);
            if ((context.Flag & ModifyStatForcedType.PDD) != 0) setPacket.WriteShort(context.PDD);
            if ((context.Flag & ModifyStatForcedType.MAD) != 0) setPacket.WriteShort(context.MAD);
            if ((context.Flag & ModifyStatForcedType.MDD) != 0) setPacket.WriteShort(context.MDD);
            if ((context.Flag & ModifyStatForcedType.EVA) != 0) setPacket.WriteShort(context.EVA);
            if ((context.Flag & ModifyStatForcedType.ACC) != 0) setPacket.WriteShort(context.ACC);
            
            if ((context.Flag & ModifyStatForcedType.Speed) != 0) setPacket.WriteByte(context.Speed);
            if ((context.Flag & ModifyStatForcedType.Jump) != 0) setPacket.WriteByte(context.Jump);
            
            if ((context.Flag & ModifyStatForcedType.SpeedMax) != 0) setPacket.WriteByte(context.SpeedMax);
            
            await _user.Dispatch(setPacket.Build());
        }
    }

    public Task Inventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryGroupContext(_user.Character.Inventories, _user.StageUser.Context.Templates.Item);
        action?.Invoke(context);
        return Inventory(context, exclRequest);
    }
    
    public async Task Inventory(IModifyInventoryGroupContext context, bool exclRequest = false)
    {
        using var packet = new PacketWriter(PacketSendOperations.InventoryOperation);
        
        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(false);

        if (context.IsUpdated)
            IsRequireUpdate = true;

        if (context.IsUpdatedAvatar)
            IsRequireUpdateAvatar = true;
        
        await _user.Dispatch(packet.Build());
    }

    public Task Skills(Action<IModifySkillContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifySkillContext(_user.Character);
        action?.Invoke(context);
        return Skills(context, exclRequest);
    }
    
    public async Task Skills(IModifySkillContext context, bool exclRequest = false)
    {
        IsRequireUpdate = true;
        
        using var packet = new PacketWriter(PacketSendOperations.ChangeSkillRecordResult);

        packet.WriteBool(exclRequest);
        packet.Write(context);
        packet.WriteBool(true);

        await _user.Dispatch(packet.Build());
    }

    public Task TemporaryStats(Action<IModifyTemporaryStatContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyTemporaryStatContext(_user.Character.TemporaryStats);
        action?.Invoke(context);
        return TemporaryStats(context, exclRequest);
    }
    
    public async Task TemporaryStats(IModifyTemporaryStatContext context, bool exclRequest = false)
    {
        var isUpdateReset = context.HistoryReset.Records.Any() ||
                            context.HistoryReset.HasTwoStateStats();
        var isUpdateSet = context.HistorySet.Records.Any() ||
                          context.HistorySet.HasTwoStateStats();
        if (!IsRequireUpdate)
            IsRequireUpdate = isUpdateReset || isUpdateSet;

        if (isUpdateReset)
        {
            using var resetLocalPacket = new PacketWriter(PacketSendOperations.TemporaryStatReset);
            using var resetRemotePacket = new PacketWriter(PacketSendOperations.UserTemporaryStatReset);

            resetLocalPacket.WriteTemporaryStatsFlag(context.HistoryReset);
            resetLocalPacket.WriteBool(false); // IsMovementAffectingStat

            resetRemotePacket.WriteInt(_user.Character.ID);
            resetRemotePacket.WriteTemporaryStatsFlag(context.HistoryReset);

            await _user.Dispatch(resetLocalPacket.Build());
            if (_user.FieldSplit != null) 
                await _user.FieldSplit.Dispatch(resetRemotePacket.Build(), _user);
        }

        if (isUpdateSet)
        {
            using var setLocalPacket = new PacketWriter(PacketSendOperations.TemporaryStatSet);
            using var setRemotePacket = new PacketWriter(PacketSendOperations.UserTemporaryStatSet);

            setLocalPacket.WriteTemporaryStatsToLocal(context.HistorySet);
            setLocalPacket.WriteShort(0); // tDelay
            setLocalPacket.WriteBool(false); // IsMovementAffectingStat

            setRemotePacket.WriteInt(_user.Character.ID);
            setRemotePacket.WriteTemporaryStatsToRemote(context.HistorySet);
            setRemotePacket.WriteShort(0); // tDelay

            await _user.Dispatch(setLocalPacket.Build());
            if (_user.FieldSplit != null) 
                await _user.FieldSplit.Dispatch(setRemotePacket.Build(), _user);
        }
    }
}
