﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Pipelines;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Plugs;

public class AbstractUserOnPacketPlug<TStageUser> : IPipelinePlug<UserOnPacket<TStageUser>> 
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPacketHandlerManager<TStageUser> _handler;

    public AbstractUserOnPacketPlug(IPacketHandlerManager<TStageUser> handler) 
        => _handler = handler;

    public Task Handle(IPipelineContext ctx, UserOnPacket<TStageUser> message) 
        => _handler.Process(message.User, message.Packet);
}
