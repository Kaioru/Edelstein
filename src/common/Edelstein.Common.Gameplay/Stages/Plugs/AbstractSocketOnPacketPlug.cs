using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Plugs;

public abstract class AbstractSocketOnPacketPlug<TStageUser> : IPipelinePlug<ISocketOnPacket<TStageUser>>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly IPacketHandlerManager<TStageUser> _handler;

    protected AbstractSocketOnPacketPlug(IPacketHandlerManager<TStageUser> handler) => _handler = handler;

    public Task Handle(IPipelineContext ctx, ISocketOnPacket<TStageUser> message) =>
        _handler.Process(message.User, message.Packet);
}
