using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Pipelines;
using MobMoveRecv = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv.MobMove;
using MobMoveSend = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send.MobMove;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketMobMove : AbstractUserOnPacketInFieldPipe<MobMoveRecv>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<MobMoveRecv> message)
    {
        var obj = message.User.Field?
            .GetPool(FieldObjectType.Mob)?
            .GetObject(message.Packet.ObjectID);

        if (obj is not IFieldMob mob || mob.Controller != message.User) return;
        if (mob.FieldSplit == null) return;
        
        var path = new FieldMobMovePath();

        path.Apply(message.Packet.Path);
        
        await mob.UpdatePosition(path);
        await message.User.Dispatch(new MobCtrlAck
        {
            ObjectID = message.Packet.ObjectID,
            MobCtrlSN = message.Packet.MobCtrlSN
        });
        await mob.FieldSplit.Dispatch(
            new MobMoveSend
            {
                ObjectID = message.Packet.ObjectID,
                MobCtrlState = message.Packet.MobCtrlState,
                RiseByToss = message.Packet.RiseByToss,
                RushMove = message.Packet.RushMove,
                DirLeft = message.Packet.DirLeft,
                Action = message.Packet.Action,
                TargetInfo = new MobMoveInfoTargetAttack
                {
                    Info = message.Packet.TargetInfo
                },
                MultiTargetForBall = message.Packet.MultiTargetForBall,
                RandTimeForAreaAttack = message.Packet.RandTimeForAreaAttack,
                Path = message.Packet.Path,
            },
            message.User
        );
    }
}
