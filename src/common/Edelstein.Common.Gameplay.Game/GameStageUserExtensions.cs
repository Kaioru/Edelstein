using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game;

public static class GameStageUserExtensions
{
    public static Task DispatchInitFuncKeys(this IGameStageUser user)
    {
        using var packet = new PacketWriter(PacketSendOperations.FuncKeyMappedInit);
        
        packet.WriteBool(user.Character?.FuncKeys.Records.Count == 0);
        if (user.Character?.FuncKeys.Records.Count > 0)
        {
            for (byte i = 0; i < 90; i++)
            {
                if (user.Character.FuncKeys.Records.TryGetValue(i, out var value))
                {
                    packet.WriteByte(value.Type);
                    packet.WriteInt(value.Action);
                }
                else
                {
                    packet.WriteByte(0);
                    packet.WriteInt(0);
                }
            }
        }

        return user.Dispatch(packet.Build());
    }
    
    public static Task DispatchInitQuickSlotKeys(this IGameStageUser user)
    {
        using var packet = new PacketWriter(PacketSendOperations.QuickslotMappedInit);
        
        packet.WriteBool(user.Character?.QuickslotKeys.Records.Count > 0);
        if (user.Character?.QuickslotKeys.Records.Count > 0)
            for (byte i = 0; i < 8; i++)
                packet.WriteInt(user.Character.QuickslotKeys.Records.TryGetValue(i, out var value) 
                    ? value 
                    : 0
                );
        return user.Dispatch(packet.Build());
    }
    
    public async static Task DispatchInitFriends(this IGameStageUser user)
    {
        if (user.Friends != null)
        {
            using var packet = new PacketWriter(PacketSendOperations.FriendResult);

            packet.WriteByte((byte)FriendResultOperations.LoadFriend_Done);
            packet.WriteByte((byte)user.Friends.Records.Count);
            foreach (var record in user.Friends.Records.Values)
                packet.WriteFriendInfo(record);
            foreach (var _ in user.Friends.Records.Values)
                packet.WriteInt(0);
            await user.Dispatch(packet.Build());
        }
    }
    
    public async static Task DispatchInitParty(this IGameStageUser user)
    {
        if (user.Party != null)
        {
            using var packet = new PacketWriter(PacketSendOperations.PartyResult);
            packet.WriteByte((byte)PartyResultOperations.LoadPartyDone);
            packet.WriteInt(user.Party.ID);
            packet.WritePartyInfo(user.Party);
            await user.Dispatch(packet.Build());
        }
    }

    public async static Task DispatchInitQuestTime(this IGameStageUser user)
    {
        var records = await user.Context.Managers.QuestTime.RetrieveAll();
        using var packet = new PacketWriter(PacketSendOperations.SetQuestTime);

        packet.WriteByte((byte)records.Count);
        foreach (var record in records)
        {
            packet.WriteInt(record.ID);
            packet.WriteDateTime(record.DateStart);
            packet.WriteDateTime(record.DateEnd);
        }
        await user.Dispatch(packet.Build());
    }
}
