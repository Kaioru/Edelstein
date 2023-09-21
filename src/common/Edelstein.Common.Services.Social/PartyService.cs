using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Foundatio.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class PartyService : IPartyService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;
    private readonly IMessageBus _messaging;

    public PartyService(IDbContextFactory<SocialDbContext> dbFactory, IMessageBus messaging)
    {
        _dbFactory = dbFactory;
        _messaging = messaging;
    }

    public async Task<PartyLoadResponse> Load(PartyLoadRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var partyMember = db.PartyMembers
                .Include(p => p.Party)
                .ThenInclude(p => p.Members)
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
    
    public async Task<PartyResponse> Create(PartyCreateRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (await db.PartyMembers.AnyAsync(m => m.CharacterID == request.CharacterID))
                return new PartyResponse(PartyResult.FailedAlreadyInParty);

            var party = new PartyEntity
            {
                BossCharacterID = request.CharacterID
            };
            var partyMember = new PartyMemberEntity
            {
                Party = party,
                CharacterID = request.CharacterID,
                CharacterName = request.CharacterName,
                Job = request.Job,
                Level = request.Level,
                ChannelID = request.ChannelID,
                FieldID = request.FieldID
            };
            
            party.Members.Add(partyMember);
            await db.Parties.AddAsync(party);
            await db.SaveChangesAsync();
            await _messaging.PublishAsync(new NotifyPartyCreated(
                request.CharacterID,
                new PartyMembership(partyMember)
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
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
            await _messaging.PublishAsync(new NotifyPartyMemberUpdateChannelOrField(
                request.PartyID,
                request.CharacterID,
                request.ChannelID,
                request.FieldID
            ));
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
            await _messaging.PublishAsync(new NotifyPartyMemberUpdateLevelOrJob(
                request.PartyID,
                request.CharacterID,
                request.Level,
                request.Job
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
}
