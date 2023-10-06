using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;

namespace Edelstein.Plugin.Rue.Contracts;

public record UserOnPacketCheckPasswordFlipped(
    ILoginStageUser User,
    string Username,
    string Password
) : UserOnPacketCheckPassword(
    User,
    Username,
    Password
);
