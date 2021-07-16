using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IMigrateableStageUser<TStage, TUser> : ISession
        where TStage : IMigrateableStage<TStage, TUser>
        where TUser : IMigrateableStageUser<TStage, TUser>
    {
        Task MigrateTo(ServerObject server);
    }
}
