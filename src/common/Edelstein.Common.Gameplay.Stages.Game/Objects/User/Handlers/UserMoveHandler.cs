using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserMoveHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserMove> _pipeline;

    public UserMoveHandler(IPipeline<IUserMove> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserMove;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadLong();
        _ = reader.ReadByte();
        _ = reader.ReadLong();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        _ = reader.ReadInt();

        var message = new UserMove(user, reader.Read(new UserMovePath()));

        return _pipeline.Process(message);
    }
}
