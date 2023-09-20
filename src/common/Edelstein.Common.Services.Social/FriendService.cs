using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Foundatio.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class FriendService : IFriendService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;
    private readonly IMessageBus _messaging;
    
    public FriendService(IDbContextFactory<SocialDbContext> dbFactory, IMessageBus messaging)
    {
        _dbFactory = dbFactory;
        _messaging = messaging;
    }

    public async Task<FriendLoadResponse> Load(FriendLoadRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var friends = db.Friends
                .Where(f => f.CharacterID == request.CharacterID)
                .ToDictionary(
                    f => f.FriendID,
                    f => (IFriend)f
                ) as IDictionary<int, IFriend>;
            var friendList = new FriendList(friends);
            return new FriendLoadResponse(FriendResult.Success, friendList);
        }
        catch (Exception)
        {
            return new FriendLoadResponse(FriendResult.FailedUnknown);
        }
    }
    
    public async Task<FriendResponse> UpdateChannel(FriendUpdateChannelRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            await db.Friends
                .Where(f => f.FriendID == request.CharacterID)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(e => e.ChannelID, e => request.ChannelID));
            await _messaging.PublishAsync(new NotifyFriendUpdateChannel(
                request.CharacterID,
                request.ChannelID
            ));
            return new FriendResponse(FriendResult.Success);
        }
        catch (Exception)
        {
            return new FriendResponse(FriendResult.FailedUnknown);
        }
    }
}
