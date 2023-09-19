using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class FriendService : IFriendService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;
    
    public FriendService(IDbContextFactory<SocialDbContext> dbFactory) => _dbFactory = dbFactory;
    
    public async Task<FriendLoadResponse> Load(FriendLoadRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var friends = db.Friends
                .Where(f => f.CharacterID == request.CharacterID)
                .ToDictionary(
                    f => f.CharacterID,
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
}
