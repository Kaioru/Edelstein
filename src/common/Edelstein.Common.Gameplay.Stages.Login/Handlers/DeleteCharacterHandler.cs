﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Contracts;
using Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class DeleteCharacterHandler : AbstractLoginPacketHandler
{
    private readonly IPipeline<IDeleteCharacter> _pipeline;

    public DeleteCharacterHandler(IPipeline<IDeleteCharacter> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.DeleteCharacter;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public override Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new DeleteCharacter(
            user,
            reader.ReadString(),
            reader.ReadInt()
        );

        return _pipeline.Process(message);
    }
}
