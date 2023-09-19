namespace Edelstein.Protocol.Services.Social.Contracts;

public record PartyLoadResponse(
    PartyResult Result,
    IPartyMembership? PartyMembership
);
