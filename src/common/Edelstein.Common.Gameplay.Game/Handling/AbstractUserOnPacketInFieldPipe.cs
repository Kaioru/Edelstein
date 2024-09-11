using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling;

public abstract class AbstractUserOnPacketInFieldPipe<TPacket> : IPipe<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, TPacket>> where TPacket : StructuredBasePacket
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, TPacket> message)
    {
        if (message.User.FieldUser == null) return;
        await HandleAfter(ctx, new PipedFieldPacketMessage<TPacket>(message.User.FieldUser, message.Packet));
    }

    protected abstract Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<TPacket> message);
}
