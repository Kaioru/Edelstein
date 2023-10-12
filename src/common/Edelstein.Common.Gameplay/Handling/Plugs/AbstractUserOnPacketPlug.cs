using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Plugs;

public abstract class AbstractUserOnPacketPlug<TStageUser> : IPipelinePlug<UserOnPacket<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPacketHandlerManager<TStageUser> _handler;

    public AbstractUserOnPacketPlug(IPacketHandlerManager<TStageUser> handler)
        => _handler = handler;

    public Task Handle(IPipelineContext ctx, UserOnPacket<TStageUser> message)
        => _handler.Process(message.User, message.Packet);
}
