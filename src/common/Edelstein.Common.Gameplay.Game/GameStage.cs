using Edelstein.Common.Gameplay.Game.Objects.User;
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

        await user.DispatchInitFuncKeys();
        await user.DispatchInitQuickSlotKeys();
        await user.DispatchInitFriends();
        await user.DispatchInitParty();
        await user.DispatchInitQuestTime();
        
        _ = user.Context.Services.Friend.UpdateChannel(new FriendUpdateChannelRequest(
            user.Character.ID,
            user.Context.Options.ChannelID
        ));
        if (user.Party != null)
            _ = user.Context.Services.Party.UpdateChannelOrField(new PartyUpdateChannelOrFieldRequest(
                user.Party.ID,
                user.Character.ID,
                user.Context.Options.ChannelID,
                field.ID
            ));

        await user.FieldUser.ModifyStatsForced(s =>
        {
            s.Reset();
            s.ACC = 1000;
        });
    }

    public new async Task Leave(IGameStageUser user)
    {
        if (user.Field != null && user.FieldUser != null)
            await user.Field.Leave(user.FieldUser);
        await base.Leave(user);
    }
}
