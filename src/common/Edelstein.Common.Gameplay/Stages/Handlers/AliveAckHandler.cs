using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Handlers
{
    public class AliveAckHandler<TStage, TUser, TConfig> : AbstractPacketHandler<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : IServerStageInfo
    {
        public override short Operation => (short)PacketRecvOperations.AliveAck;

        public override Task<bool> Check(TUser user)
            => Task.FromResult(!user.IsMigrating);

        public override Task Handle(TUser user, IPacketReader packet)
            => user.TryRecvAliveAck();
    }
}
