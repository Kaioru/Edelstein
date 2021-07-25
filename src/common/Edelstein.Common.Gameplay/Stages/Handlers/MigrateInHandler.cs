using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Handlers
{
    public class MigrateInHandler<TStage, TUser> : AbstractPacketHandler<TStage, TUser>
        where TStage : AbstractMigrateableStage<TStage, TUser>
        where TUser : AbstractMigrateableStageUser<TStage, TUser>
    {
        private readonly TStage _stage;

        public override short Operation => (short)PacketRecvOperations.MigrateIn;

        public MigrateInHandler(TStage stage)
            => _stage = stage;

        public override Task<bool> Check(TUser user)
            => Task.FromResult(!user.IsMigrating && !user.IsLoggingIn);

        public override async Task Handle(TUser user, IPacketReader packet)
        {
            var character = packet.ReadInt();
            var result = await user.MigrateIn(character, 0); // TODO: user key

            if (result) await _stage.Enter(user);
            else await user.Disconnect();
        }
    }
}
