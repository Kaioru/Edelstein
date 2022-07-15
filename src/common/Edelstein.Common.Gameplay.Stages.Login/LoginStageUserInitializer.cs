using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUserInitializer : IAdapterInitializer
{
    public IAdapter Initialize(ISocket socket)
        => new LoginStageUser(socket);
}
