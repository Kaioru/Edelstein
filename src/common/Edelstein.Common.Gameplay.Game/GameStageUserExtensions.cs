using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game;

public static class GameStageUserExtensions
{
    public static Task DispatchInitFuncKeys(this IGameStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.FuncKeyMappedInit);
        
        p.WriteBool(user.Character?.FuncKeys.Records.Count == 0);
        if (user.Character?.FuncKeys.Records.Count > 0)
        {
            for (byte i = 0; i < 90; i++)
            {
                if (user.Character.FuncKeys.Records.TryGetValue(i, out var value))
                {
                    p.WriteByte(value.Type);
                    p.WriteInt(value.Action);
                }
                else
                {
                    p.WriteByte(0);
                    p.WriteInt(0);
                }
            }
        }

        return user.Dispatch(p.Build());
    }
    
    public static Task DispatchInitQuickSlotKeys(this IGameStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.QuickslotMappedInit);
        
        p.WriteBool(user.Character?.QuickslotKeys.Records.Count > 0);
        if (user.Character?.QuickslotKeys.Records.Count > 0)
            for (byte i = 0; i < 8; i++)
                p.WriteInt(user.Character.QuickslotKeys.Records.TryGetValue(i, out var value) 
                    ? value 
                    : 0
                );
        return user.Dispatch(p.Build());
    }
    
    public async static Task DispatchInitFriends(this IGameStageUser user)
    {
        if (user.Friends != null)
        {
            var p = new PacketWriter(PacketSendOperations.FriendResult);

            p.WriteByte((byte)FriendResultOperations.LoadFriend_Done);
            p.WriteByte((byte)user.Friends.Records.Count);
            foreach (var record in user.Friends.Records.Values)
                p.WriteFriendInfo(record);
            foreach (var _ in user.Friends.Records.Values)
                p.WriteInt(0);
            await user.Dispatch(p.Build());
        }
    }
    
    public async static Task DispatchInitParty(this IGameStageUser user)
    {
        if (user.Party != null)
        {
            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.LoadPartyDone);
            p.WriteInt(user.Party.ID);
            p.WritePartyInfo(user.Party);
            await user.Dispatch(p.Build());
        }
    }

    public async static Task DispatchInitQuestTime(this IGameStageUser user)
    {
        var records = await user.Context.Managers.QuestTime.RetrieveAll();
        var p = new PacketWriter(PacketSendOperations.SetQuestTime);

        p.WriteByte((byte)records.Count);
        foreach (var record in records)
        {
            p.WriteInt(record.ID);
            p.WriteDateTime(record.DateStart);
            p.WriteDateTime(record.DateEnd);
        }
        await user.Dispatch(p.Build());
    }
}
