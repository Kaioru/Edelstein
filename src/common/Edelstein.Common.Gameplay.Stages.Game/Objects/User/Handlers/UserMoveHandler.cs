using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserMoveHandler : AbstractFieldUserHandler
{
    public override short Operation => (short)PacketRecvOperations.UserMove;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        _ = reader.ReadByte();
        var path = reader.Read(new UserMovePath());

        return user.Move(path);
    }
}
