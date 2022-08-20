using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextEvents(
    IEvent<IUserEnterStage<ILoginStageUser>> UserEnterStage,
    IEvent<IUserLeaveStage<ILoginStageUser>> UserLeaveStage
) : ILoginContextEvents;
