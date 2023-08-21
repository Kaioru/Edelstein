namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketSelectWorld(
    ILoginStageUser User,
    int WorldID,
    int ChannelID
);
