using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IMigrateableStageUser<TStage, TUser> : IStageUser<TStage, TUser>
        where TStage : IMigrateableStage<TStage, TUser>
        where TUser : IMigrateableStageUser<TStage, TUser>
    {
        Task<bool> MigrateIn(int character, long key);

        Task<bool> MigrateTo(string server);
        Task<bool> MigrateTo(ServerObject server);

        Task TrySendAliveReq();
        Task TryRecvAliveAck();
    }
}
