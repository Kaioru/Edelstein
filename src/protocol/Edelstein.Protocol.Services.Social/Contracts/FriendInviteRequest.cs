namespace Edelstein.Protocol.Services.Social.Contracts;

public record FriendInviteRequest(
    int InviterID,
    string InviterName,
    int InviterLevel,
    int InviterJob,
    int InviterChannelID,
    string FriendName,
    string FriendGroup
);
