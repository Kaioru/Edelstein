using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;

namespace Edelstein.Common.Gameplay.Stages.Login.Messages;

public record LogoutWorld(
    ILoginStageUser User
) : ILogoutWorld;
