using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Protocol.Services.Social;

public interface IFriendService
{
    Task<FriendLoadResponse> Load(FriendLoadRequest request);
}
