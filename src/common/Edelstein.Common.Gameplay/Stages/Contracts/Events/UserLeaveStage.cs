using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Events;

public record UserLeaveStage<TStageUser>(
    TStageUser User
) : IUserLeaveStage<TStageUser> where TStageUser : IStageUser<TStageUser>;
