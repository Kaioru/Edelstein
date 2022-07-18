using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class SocketOnDisconnectAction : AbstractSocketOnDisconnectAction<ILoginStageUser>
{
    public SocketOnDisconnectAction(ISessionService sessions) : base(sessions)
    {
    }
}
