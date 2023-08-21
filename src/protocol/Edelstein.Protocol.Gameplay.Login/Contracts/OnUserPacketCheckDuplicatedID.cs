namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketCheckDuplicatedID(
    ILoginStageUser User,
    string Name
);
