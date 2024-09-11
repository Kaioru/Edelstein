using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;
using UserMoveRecv = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv.UserMove;
using UserMoveSend = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send.UserMove;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserMove : AbstractUserOnPacketInFieldPipe<UserMoveRecv>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserMoveRecv> message)
    {
        if (message.User.FieldSplit == null) return;

        var path = new FieldUserMovePath();

        path.Apply(message.Packet.Path);

        await message.User.UpdatePosition(path);
        await message.User.FieldSplit.Dispatch(
            new UserMoveSend
            {
                ObjectID = message.User.ObjectID ?? 0,
                Path = message.Packet.Path
            },
            message.User
        );
    }
}
