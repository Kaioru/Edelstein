using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Characters.Skills.Modify;
using Edelstein.Common.Gameplay.Models.Characters.Stats;
using Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
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
        
        var packet = new PacketWriter(PacketSendOperations.StatChanged);

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
        
        var packet = new PacketWriter(PacketSendOperations.ChangeSkillRecordResult);

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
            var resetLocalPacket = new PacketWriter(PacketSendOperations.TemporaryStatReset);
            var resetRemotePacket = new PacketWriter(PacketSendOperations.UserTemporaryStatReset);

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
            var setLocalPacket = new PacketWriter(PacketSendOperations.TemporaryStatSet);
            var setRemotePacket = new PacketWriter(PacketSendOperations.UserTemporaryStatSet);

            setLocalPacket.WriteTemporaryStatsToLocal(context.HistorySet);
            setLocalPacket.WriteShort(0); // tDelay
            setLocalPacket.WriteBool(false); // IsMovementAffectingStat

            setRemotePacket.WriteInt(_user.Character.ID);
            setRemotePacket.WriteTemporaryStatsToLocal(context.HistorySet);
            setRemotePacket.WriteShort(0); // tDelay

            await _user.Dispatch(setLocalPacket.Build());
            if (_user.FieldSplit != null) 
                await _user.FieldSplit.Dispatch(setRemotePacket.Build(), _user);
        }
    }
}
