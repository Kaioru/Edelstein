namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketCheckUserLimit(
    ILoginStageUser User,
    int WorldID
);
