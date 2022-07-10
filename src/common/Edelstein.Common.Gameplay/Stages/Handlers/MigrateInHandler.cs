using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Handlers
{
    public class MigrateInHandler<TStage, TUser, TConfig> : AbstractPacketHandler<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageInfo
    {
        public override short Operation => (short)PacketRecvOperations.MigrateIn;
        private readonly TStage _stage;

        public MigrateInHandler(TStage stage)
            => _stage = stage;

        public override Task<bool> Check(TUser user)
            => Task.FromResult(!user.IsMigrating && !user.IsLoggingIn);

        public override async Task Handle(TUser user, IPacketReader packet)
        {
            var character = packet.ReadInt();
            _ = packet.ReadBytes(18); // Unknown
            var key = packet.ReadLong();

            var claim = await _stage.MigrationRegistry.Claim(new ClaimMigrationRequest
            {
                Character = character,
                ClientKey = key,
                Server = _stage.ID
            });

            if (claim.Result != MigrationRegistryResult.Ok)
            {
                await user.Disconnect();
                return;
            }

            user.Character = await _stage.CharacterRepository.Retrieve(character);
            user.AccountWorld = await _stage.AccountWorldRepository.Retrieve(user.Character.AccountWorldID);
            user.Account = await _stage.AccountRepository.Retrieve(user.AccountWorld.AccountID);

            user.Key = claim.Migration.ClientKey;

            await _stage.Enter(user);
        }
    }
}
