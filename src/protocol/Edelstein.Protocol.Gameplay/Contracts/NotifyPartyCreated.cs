using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyCreated(
    int CharacterID,
    IPartyMembership Party
);
