﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Buffers.Bytes;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class WorldInfoRequestHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IWorldRequest> _pipeline;

    public WorldInfoRequestHandler(IPipeline<IWorldRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.WorldInfoRequest;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IByteReader reader) =>
        _pipeline.Process(new WorldRequest(user));
}