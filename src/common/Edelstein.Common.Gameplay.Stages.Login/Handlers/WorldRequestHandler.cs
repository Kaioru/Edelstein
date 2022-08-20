﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class WorldRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IWorldRequest> _pipeline;

    public WorldRequestHandler(IPipeline<IWorldRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.WorldRequest;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader) =>
        _pipeline.Process(new WorldRequest(user));
}
