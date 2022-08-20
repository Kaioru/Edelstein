using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;

namespace Edelstein.Common.Gameplay.Stages.Contracts.Events;

public record UserEnterStage<TStageUser>(
    TStageUser User
) : IUserEnterStage<TStageUser> where TStageUser : IStageUser<TStageUser>;
