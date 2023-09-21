namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendInviteRejectRequest(
    int InviterID,
    int CharacterID
);
