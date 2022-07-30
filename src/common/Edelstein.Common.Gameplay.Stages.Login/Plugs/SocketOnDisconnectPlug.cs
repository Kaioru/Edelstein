using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SocketOnDisconnectPlug : AbstractSocketOnDisconnectPlug<ILoginStageUser>
{
    public SocketOnDisconnectPlug(ISessionService session) : base(session)
    {
    }
}
