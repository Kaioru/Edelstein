namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketCheckPassword(
    ILoginStageUser User,
    string Username,
    string Password
);
