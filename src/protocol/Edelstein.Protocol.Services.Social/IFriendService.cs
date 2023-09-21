using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Protocol.Services.Social;

public interface IFriendService
{
    Task<FriendLoadResponse> Load(FriendLoadRequest request);

    Task<FriendResponse> Invite(FriendInviteRequest request);
    Task<FriendResponse> InviteAccept(FriendInviteAcceptRequest request);
    Task<FriendResponse> Delete(FriendDeleteRequest request);

    Task<FriendResponse> UpdateProfile(FriendUpdateProfileRequest request);
    Task<FriendResponse> UpdateGroup(FriendUpdateGroupRequest request);
    Task<FriendResponse> UpdateChannel(FriendUpdateChannelRequest request);
}
