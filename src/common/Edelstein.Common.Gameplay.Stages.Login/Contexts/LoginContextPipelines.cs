using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<IStageUserOnPacket<ILoginStageUser>> StageUserOnPacket,
    IPipeline<IStageUserOnException<ILoginStageUser>> StageUserOnException,
    IPipeline<IStageUserOnDisconnect<ILoginStageUser>> StageUserOnDisconnect
) : ILoginContextPipelines;
