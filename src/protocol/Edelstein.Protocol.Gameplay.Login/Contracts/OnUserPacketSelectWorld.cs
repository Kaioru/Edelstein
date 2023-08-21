namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketSelectWorld(
    ILoginStageUser User,
    int WorldID,
    int ChannelID
);
