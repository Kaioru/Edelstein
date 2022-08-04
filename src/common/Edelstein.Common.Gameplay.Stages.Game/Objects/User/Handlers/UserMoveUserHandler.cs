using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserMoveUserHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserMove> _pipeline;

    public UserMoveUserHandler(IPipeline<IUserMove> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserMove;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadLong();
        _ = reader.ReadByte();
        _ = reader.ReadLong();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        _ = reader.ReadInt();

        var message = new UserMove(user, reader.Read(new MovePath()));

        return _pipeline.Process(message);
    }
}
