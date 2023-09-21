using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyMemberJoined(
    int PartyID,
    IPartyMembership Party,
    IPartyMember PartyMember
);
