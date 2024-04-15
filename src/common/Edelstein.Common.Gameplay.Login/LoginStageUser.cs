using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageUser(
    ILoginStageSystem system, 
    ISocket socket
) : ILoginStageUser
{
    public ILoginStageSystem System { get; } = system;
    public ISocket Socket { get; } = socket;
}
