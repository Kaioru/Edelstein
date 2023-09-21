using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Protocol.Services.Social;

public interface IPartyService
{
    Task<PartyLoadResponse> Load(PartyLoadRequest request);

    Task<PartyResponse> Create(PartyCreateRequest request);
    Task<PartyResponse> Disband(PartyDisbandRequest request);
    Task<PartyResponse> Leave(PartyLeaveRequest request);
    
    Task<PartyResponse> Invite(PartyInviteRequest request);
    Task<PartyResponse> InviteAccept(PartyInviteAcceptRequest request);
    Task<PartyResponse> InviteReject(PartyInviteRejectRequest request);

    Task<PartyResponse> Kick(PartyKickRequest request);

    Task<PartyResponse> ChangeBoss(PartyChangeBossRequest request);

    Task<PartyResponse> UpdateChannelOrField(PartyUpdateChannelOrFieldRequest request);
    Task<PartyResponse> UpdateLevelOrJob(PartyUpdateLevelOrJobRequest request);
}
