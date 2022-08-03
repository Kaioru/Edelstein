using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers;

public class UserTransferChannelRequestHandler : AbstractFieldHandler
{
    private readonly IPipeline<IUserTransferChannelRequest> _pipeline;

    public UserTransferChannelRequestHandler(IPipeline<IUserTransferChannelRequest> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserTransferChannelRequest;

    public override bool Check(IGameStageUser user) => base.Check(user) && !user.IsMigrating;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var message = new UserTransferChannelRequest(
            user,
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
