using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record AuthLoginBasic(
    ILoginStageUser User,
    string Username,
    string Password
) : IAuthLoginBasic;
