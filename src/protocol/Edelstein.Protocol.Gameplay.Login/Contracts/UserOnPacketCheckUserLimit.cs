namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketCheckUserLimit(
    ILoginStageUser User,
    int WorldID
);
