namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketCheckDuplicatedID(
    ILoginStageUser User,
    string Name
);
