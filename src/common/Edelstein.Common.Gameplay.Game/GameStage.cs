using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Common.Gameplay.Game;

public class GameStage : AbstractStage<IGameStageUser>, IGameStage
{
    private readonly IFieldManager _fieldManager;

    public GameStage(string id, IFieldManager fieldManager)
    {
        ID = id;
        _fieldManager = fieldManager;
    }

    public override string ID { get; }

    public new async Task Enter(IGameStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }

        var field = await _fieldManager.Retrieve(user.Character.FieldID);
        var fieldUser = new FieldUser(user, user.Account, user.AccountWorld, user.Character);

        if (field == null)
        {
            await user.Disconnect();
            return;
        }

        await user.Context.Services.Friend.UpdateProfile(new FriendUpdateProfileRequest(
            user.Character.ID,
            user.Character.FriendMax,
            user.Account.GradeCode > 0 || user.Account.SubGradeCode > 0
        ));

        user.Friends = (await user.Context.Services.Friend.Load(new FriendLoadRequest(user.Character.ID))).Friends;
        user.Party = (await user.Context.Services.Party.Load(new PartyLoadRequest(user.Character.ID))).PartyMembership;

        user.FieldUser = fieldUser;
        
        await field.Enter(fieldUser);
        await base.Enter(user);

        var funcKeyMappedInitPacket = new PacketWriter(PacketSendOperations.FuncKeyMappedInit);
        
        funcKeyMappedInitPacket.WriteBool(user.Character.FuncKeys.Records.Count == 0);
        if (user.Character.FuncKeys.Records.Count > 0)
        {
            for (byte i = 0; i < 90; i++)
            {
                if (user.Character.FuncKeys.Records.TryGetValue(i, out var value))
                {
                    funcKeyMappedInitPacket.WriteByte(value.Type);
                    funcKeyMappedInitPacket.WriteInt(value.Action);
                }
                else
                {
                    funcKeyMappedInitPacket.WriteByte(0);
                    funcKeyMappedInitPacket.WriteInt(0);
                }
            }
        }
        await fieldUser.Dispatch(funcKeyMappedInitPacket.Build());
        
        var quickslotMappedInitPacket = new PacketWriter(PacketSendOperations.QuickslotMappedInit);
        
        quickslotMappedInitPacket.WriteBool(user.Character.QuickslotKeys.Records.Count > 0);
        if (user.Character.QuickslotKeys.Records.Count > 0)
            for (byte i = 0; i < 8; i++)
                quickslotMappedInitPacket.WriteInt(user.Character.QuickslotKeys.Records.TryGetValue(i, out var value) 
                    ? value 
                    : 0
                );
        await fieldUser.Dispatch(quickslotMappedInitPacket.Build());

        if (user.Friends != null)
        {
            var friendPacket = new PacketWriter(PacketSendOperations.FriendResult);

            friendPacket.WriteByte((byte)FriendResultOperations.LoadFriend_Done);
            friendPacket.WriteByte((byte)user.Friends.Records.Count);
            foreach (var record in user.Friends.Records.Values)
                friendPacket.WriteFriendInfo(record);
            foreach (var _ in user.Friends.Records.Values)
                friendPacket.WriteInt(0);
            await fieldUser.Dispatch(friendPacket.Build());
            await user.Context.Services.Friend.UpdateChannel(new FriendUpdateChannelRequest(
                user.Character.ID,
                user.Context.Options.ChannelID
            ));
        }
        
        if (user.Party != null)
        {
            var partyPacket = new PacketWriter(PacketSendOperations.PartyResult);
            partyPacket.WriteByte((byte)PartyResultOperations.LoadPartyDone);
            partyPacket.WriteInt(user.Party.ID);
            partyPacket.WritePartyInfo(user.Party);
            await fieldUser.Dispatch(partyPacket.Build());
            await user.Context.Services.Party.UpdateChannelOrField(new PartyUpdateChannelOrFieldRequest(
                user.Party.ID,
                user.Character.ID,
                user.Context.Options.ChannelID,
                field.ID
            ));
        }
    }

    public new async Task Leave(IGameStageUser user)
    {
        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
        await base.Leave(user);
    }
}
