using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;

namespace Edelstein.Common.Gameplay.Stages.Login.Messages;

public record CheckPassword(
    ILoginStageUser User,
    string Username,
    string Password
) : ILoginCheckPassword;
