﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class WorldRequestHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketWorldRequest> _pipeline;

    public WorldRequestHandler(IPipeline<UserOnPacketWorldRequest> pipeline) => _pipeline = pipeline;
    public short Operation => (short)PacketRecvOperations.WorldRequest;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public Task Handle(ILoginStageUser user, IPacketReader reader) =>
        _pipeline.Process(new UserOnPacketWorldRequest(user));
}
