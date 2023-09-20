using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyFriendUpdateList(
    int CharacterID,
    IFriendList FriendList
);
