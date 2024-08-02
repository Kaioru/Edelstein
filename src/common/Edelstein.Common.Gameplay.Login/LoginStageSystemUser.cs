using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystemUser(
    ISocket socket, 
    ILoginStageSystem system
) : ILoginStageSystemUser
{
    public ISocket Socket { get; } = socket;
    public ILoginStageSystem System { get; } = system;
}
