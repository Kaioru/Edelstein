using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Services.Social.Contracts;
using Foundatio.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Social;

public class PartyService : IPartyService
{
    private readonly IDbContextFactory<SocialDbContext> _dbFactory;
    private readonly IMessageBus _messaging;
    private readonly ICharacterRepository _characterRepository;

    public PartyService(IDbContextFactory<SocialDbContext> dbFactory, IMessageBus messaging, ICharacterRepository characterRepository)
    {
        _dbFactory = dbFactory;
        _messaging = messaging;
        _characterRepository = characterRepository;
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

    public async Task<PartyResponse> Disband(PartyDisbandRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (!await db.Parties.AnyAsync(p => p.ID == request.PartyID && p.BossCharacterID == request.CharacterID))
                return new PartyResponse(PartyResult.FailedNotBoss);
            await db.Parties
                .Where(p => p.ID == request.PartyID)
                .ExecuteDeleteAsync();
            await _messaging.PublishAsync(new NotifyPartyDisbanded(
                request.CharacterID,
                request.PartyID
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
    
    public async Task<PartyResponse> Leave(PartyLeaveRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (await db.Parties.AnyAsync(p => p.ID == request.PartyID && p.BossCharacterID == request.CharacterID))
                return new PartyResponse(PartyResult.FailedIsBoss);
            await db.PartyMembers
                .Where(p => p.PartyID == request.PartyID && p.CharacterID == request.CharacterID)
                .ExecuteDeleteAsync();
            await _messaging.PublishAsync(new NotifyPartyMemberWithdrawn(
                request.PartyID,
                request.CharacterID,
                request.CharacterName,
                false
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }

    public async Task<PartyResponse> Invite(PartyInviteRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (!await db.Parties.AnyAsync(p => p.ID == request.PartyID && p.BossCharacterID == request.InviterID))
                return new PartyResponse(PartyResult.FailedNotBoss);
            
            var target = await _characterRepository.RetrieveByName(request.CharacterName);
            var now = DateTime.UtcNow;
            
            if (target == null)
                return new PartyResponse(PartyResult.FailedCharacterNotFound);
            
            if (request.InviterID == target.ID)
                return new PartyResponse(PartyResult.FailedSelf);
            
            if (await db.PartyMembers.AnyAsync(m => m.CharacterID == target.ID))
                return new PartyResponse(PartyResult.FailedAlreadyInParty);

            if (await db.PartyInvitations.AnyAsync(i =>
                    i.PartyID == request.PartyID &&
                    i.CharacterID == target.ID &&
                    i.DateExpire > now))
                return new PartyResponse(PartyResult.FailedAlreadyInvited);
            
            await db.PartyInvitations
                .Where(i => i.PartyID == request.PartyID && i.CharacterID == target.ID)
                .ExecuteDeleteAsync();
            await db.PartyInvitations.AddAsync(new PartyInvitationEntity
            {
                PartyID = request.PartyID,
                CharacterID = target.ID,
                DateExpire = now.AddMinutes(3)
            });
            await db.SaveChangesAsync();
            await _messaging.PublishAsync(new NotifyPartyMemberInvited(
                request.InviterID,
                request.InviterName,
                request.InviterLevel,
                request.InviterJob,
                request.PartyID,
                target.ID
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
    
    public async Task<PartyResponse> InviteAccept(PartyInviteAcceptRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var invitation = await db.PartyInvitations
                .FirstOrDefaultAsync(i => i.PartyID == request.PartyID && i.CharacterID == request.CharacterID);
            
            if (invitation == null || invitation.DateExpire < now)
                return new PartyResponse(PartyResult.FailedNotInvited);
            if (await db.PartyMembers.AnyAsync(m => m.PartyID == request.PartyID && m.CharacterID == request.CharacterID))
                return new PartyResponse(PartyResult.FailedAlreadyInParty);
            if (await db.PartyMembers.Where(m => m.PartyID == request.PartyID).CountAsync() >= 6)
                return new PartyResponse(PartyResult.FailedFull);
            
            var partyMember = new PartyMemberEntity
            {
                PartyID = request.PartyID,
                CharacterID = request.CharacterID,
                CharacterName = request.CharacterName,
                Job = request.Job,
                Level = request.Level,
                ChannelID = request.ChannelID,
                FieldID = request.FieldID
            };

            db.PartyInvitations.Remove(invitation);
            await db.PartyMembers.AddAsync(partyMember);
            await db.SaveChangesAsync();

            partyMember = await db.PartyMembers
                .Include(m => m.Party)
                .ThenInclude(p => p.Members)
                .FirstAsync(m => m.PartyID == request.PartyID && m.CharacterID == request.CharacterID);
            
            await _messaging.PublishAsync(new NotifyPartyMemberJoined(
                request.PartyID,
                new PartyMembership(partyMember),
                new PartyMembershipMember(partyMember)
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }

    public async Task<PartyResponse> InviteReject(PartyInviteRejectRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var invitation = await db.PartyInvitations
                .FirstOrDefaultAsync(i => i.PartyID == request.PartyID && i.CharacterID == request.CharacterID);
            if (invitation == null || invitation.DateExpire < now)
                return new PartyResponse(PartyResult.FailedNotInvited);
            db.PartyInvitations.Remove(invitation);
            await db.SaveChangesAsync();
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
    public async Task<PartyResponse> Kick(PartyKickRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (request.BossID == request.CharacterID)
                return new PartyResponse(PartyResult.FailedSelf);
            if (!await db.Parties.AnyAsync(p => p.ID == request.PartyID && p.BossCharacterID == request.BossID))
                return new PartyResponse(PartyResult.FailedNotBoss);
            var partyMember = db.PartyMembers
                .FirstOrDefault(p => p.PartyID == request.PartyID && p.CharacterID == request.CharacterID);
            
            if (partyMember == null)
                return new PartyResponse(PartyResult.FailedNotInParty);

            db.Remove(partyMember);
            await db.SaveChangesAsync();
            await _messaging.PublishAsync(new NotifyPartyMemberWithdrawn(
                request.PartyID,
                request.CharacterID,
                partyMember.CharacterName,
                true
            ));
            return new PartyResponse(PartyResult.Success);
        }
        catch (Exception)
        {
            return new PartyResponse(PartyResult.FailedUnknown);
        }
    }
    
    public async Task<PartyResponse> ChangeBoss(PartyChangeBossRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            
            if (request.BossID == request.CharacterID)
                return new PartyResponse(PartyResult.FailedSelf);
            if (!await db.Parties.AnyAsync(p => p.ID == request.PartyID && p.BossCharacterID == request.BossID))
                return new PartyResponse(PartyResult.FailedNotBoss);
            
            var partyMember = db.PartyMembers
                .FirstOrDefault(p => p.PartyID == request.PartyID && p.CharacterID == request.CharacterID);
            
            if (partyMember == null)
                return new PartyResponse(PartyResult.FailedNotInParty);
            if (partyMember.ChannelID < 0)
                return new PartyResponse(PartyResult.FailedOffline);
            
            await db.Parties
                .Where(p => p.ID == request.PartyID)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(e => e.BossCharacterID, e => request.CharacterID));
            await _messaging.PublishAsync(new NotifyPartyChangedBoss(
                request.PartyID,
                request.CharacterID,
                request.IsDisconnected
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
