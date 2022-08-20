using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStage : AbstractStage<ILoginStageUser>, ILoginStage
{
    public LoginStage(
        IEvent<IUserEnterStage<ILoginStageUser>> enterEvent,
        IEvent<IUserLeaveStage<ILoginStageUser>> leaveEvent
    ) : base(enterEvent, leaveEvent)
    {
    }
}
