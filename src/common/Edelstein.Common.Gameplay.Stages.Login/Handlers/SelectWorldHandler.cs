﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class SelectWorldHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<ISelectWorld> _pipeline;

    public SelectWorldHandler(IPipeline<ISelectWorld> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.SelectWorld;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte(); // Unknown1

        var message = new SelectWorld(
            user,
            reader.ReadByte(),
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
