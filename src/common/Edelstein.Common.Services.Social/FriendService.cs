using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Foundatio.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class FriendService : IFriendService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;
    private readonly IMessageBus _messaging;
    private readonly ICharacterRepository _characterRepository;
    
    public FriendService(IDbContextFactory<SocialDbContext> dbFactory, IMessageBus messaging, ICharacterRepository characterRepository)
    {
        _dbFactory = dbFactory;
        _messaging = messaging;
        _characterRepository = characterRepository;
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
    
    public async Task<FriendResponse> Invite(FriendInviteRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var target = await _characterRepository.RetrieveByName(request.FriendName);
            
            if (target == null)
                return new FriendResponse(FriendResult.FailedCharacterNotFound);

            if (await db.Friends.AnyAsync(f => 
                    f.CharacterID == request.InviterID && f.FriendID == target.ID || 
                    f.CharacterID == target.ID && f.FriendID == request.InviterID))
                return new FriendResponse(FriendResult.FailedAlreadyAdded);

            var inviterFriendCount = await db.Friends.CountAsync(f => f.CharacterID == request.InviterID);
            var targetFriendCount = await db.Friends.CountAsync(f => f.CharacterID == target.ID);
            
            /*
            if (await db.FriendProfiles.AnyAsync(p => p.CharacterID == request.InviterID && p.FriendMax >= inviterFriendCount) ||
                await db.FriendProfiles.AnyAsync(p => p.CharacterID == target.ID && p.FriendMax >= targetFriendCount))
                return new FriendResponse(FriendResult.FailedMaxSlot);*/
            
            var record1 = new FriendEntity
            {
                CharacterID = request.InviterID,
                FriendID = target.ID,
                FriendName = target.Name,
                FriendGroup = request.FriendGroup,
                Flag = 2,
                ChannelID = -1
            };
            var record2 = new FriendEntity
            {
                CharacterID = target.ID,
                FriendID = request.InviterID,
                FriendName = request.InviterName,
                FriendGroup = "Group Unknown",
                Flag = 1,
                ChannelID = request.InviterChannelID
            };
            
            await db.Friends.AddAsync(record1);
            await db.Friends.AddAsync(record2);
            await db.SaveChangesAsync();

            var friends = db.Friends
                .Where(f => f.CharacterID == request.InviterID)
                .ToDictionary(
                    f => f.FriendID,
                    f => (IFriend)f
                );
            var friendList = new FriendList(friends);

            await _messaging.PublishAsync(new NotifyFriendUpdateList(
                request.InviterID,
                friendList
            ));
            await _messaging.PublishAsync(new NotifyFriendInvited(
                request.InviterID,
                request.InviterName,
                request.InviterLevel,
                request.InviterJob,
                target.ID,
                record2
            ));
            return new FriendResponse(FriendResult.Success);
        }
        catch (Exception)
        {
            return new FriendResponse(FriendResult.FailedUnknown);
        }
    }

    public async Task<FriendResponse> UpdateProfile(FriendProfileRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var profile = db.FriendProfiles.Any(f => f.CharacterID == request.CharacterID);

            if (profile)
                await db.FriendProfiles
                    .Where(f => f.CharacterID == request.CharacterID)
                    .ExecuteUpdateAsync(p => p
                        .SetProperty(e => e.FriendMax, e => request.FriendMax)
                        .SetProperty(e => e.IsMaster, e => request.IsMaster));
            else
            {
                await db.FriendProfiles.AddAsync(new FriendProfileEntity
                {
                    CharacterID = request.CharacterID,
                    FriendMax = request.FriendMax,
                    IsMaster = request.IsMaster
                });
                await db.SaveChangesAsync();
            }
            return new FriendResponse(FriendResult.Success);
        }
        catch (Exception)
        {
            return new FriendResponse(FriendResult.FailedUnknown);
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
