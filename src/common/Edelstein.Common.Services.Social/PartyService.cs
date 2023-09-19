using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class PartyService : IPartyService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;

    public PartyService(IDbContextFactory<SocialDbContext> dbFactory) => _dbFactory = dbFactory;
    
    public async Task<PartyLoadResponse> Load(PartyLoadRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var partyMember = db.PartyMembers
                .Include(p => p.Party)
                .FirstOrDefault(p => p.CharacterID == request.CharacterID);
            return new PartyLoadResponse(PartyResult.Success, partyMember != null
                ? new PartyMembership(partyMember)
                : null);
        }
        catch (Exception)
        {
            return new PartyLoadResponse(PartyResult.FailedUnknown);
        }
    }
    
    public async Task<PartyResponse> UpdateChannelOrField(PartyUpdateChannelOrFieldRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            await db.PartyMembers
                .Where(p => p.PartyID == request.PartyID && p.CharacterID == request.CharacterID)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(e => e.ChannelID, e => request.ChannelID)
                    .SetProperty(e => e.FieldID, e => request.FieldID));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }

    public async Task<PartyResponse> UpdateLevelOrJob(PartyUpdateLevelOrJobRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            await db.PartyMembers
                .Where(p => p.PartyID == request.PartyID && p.CharacterID == request.CharacterID)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(e => e.Level, e => request.Level)
                    .SetProperty(e => e.Job, e => request.Job));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
}
