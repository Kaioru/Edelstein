using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystem(
    ILoginStageSystemOptions options
) : AbstractStageSystem<ILoginStageUser, ILoginStageSystem>, ILoginStageSystem
{
    public override string ID => options.ID;
    public ILoginStageSystemOptions Options => options;

    public override ILoginStageUser CreateUser(ISocket socket)
        => new LoginStageUser(this, socket);
}
