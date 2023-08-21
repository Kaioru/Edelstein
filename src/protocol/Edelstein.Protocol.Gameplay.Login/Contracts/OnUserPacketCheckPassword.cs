namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketCheckPassword(
    ILoginStageUser User,
    string Username,
    string Password
);
