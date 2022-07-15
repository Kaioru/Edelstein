using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record StageUserOnException<TStageUser>(
    TStageUser User,
    Exception Exception
) : IStageUserOnException<TStageUser> where TStageUser : IStageUser;
