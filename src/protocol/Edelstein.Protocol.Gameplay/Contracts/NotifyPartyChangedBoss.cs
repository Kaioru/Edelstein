namespace Edelstein.Protocol.Gameplay.Contracts;

public record NotifyPartyChangedBoss(
    int PartyID,
    int BossID,
    bool IsDisconnected
);
