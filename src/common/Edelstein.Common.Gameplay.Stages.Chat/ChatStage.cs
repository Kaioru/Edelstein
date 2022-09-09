using Edelstein.Protocol.Gameplay.Stages.Chat;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages.Chat;

public class ChatStage : AbstractStage<IChatStageUser>, IChatStage
{
    public ChatStage(
        IEvent<IUserEnterStage<IChatStageUser>> enterEvent,
        IEvent<IUserLeaveStage<IChatStageUser>> leaveEvent
    ) : base(enterEvent, leaveEvent)
    {
    }
}
