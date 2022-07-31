﻿using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<ISocketOnPacket<ILoginStageUser>> SocketOnPacket,
    IPipeline<ISocketOnException<ILoginStageUser>> SocketOnException,
    IPipeline<ISocketOnDisconnect<ILoginStageUser>> SocketOnDisconnect,
    IPipeline<ICheckPassword> CheckPassword,
    IPipeline<ICheckUserLimit> CheckUserLimit,
    IPipeline<IWorldRequest> WorldRequest
) : ILoginContextPipelines;
