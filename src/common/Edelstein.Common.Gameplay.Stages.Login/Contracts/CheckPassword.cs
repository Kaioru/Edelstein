using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts;

public record CheckPassword(
    ILoginStageUser User,
    string Username,
    string Password
) : ICheckPassword;
