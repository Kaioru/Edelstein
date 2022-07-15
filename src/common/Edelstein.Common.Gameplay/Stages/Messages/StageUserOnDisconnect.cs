using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Common.Gameplay.Stages.Messages;

public record StageUserOnDisconnect<TStageUser>(
    TStageUser User
) : IStageUserOnDisconnect<TStageUser> where TStageUser : IStageUser;
