using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IServerStageUser<TStage, TUser> : IStageUser<TStage, TUser>
        where TStage : IServerStage<TStage, TUser>
        where TUser : IServerStageUser<TStage, TUser>
    {
        Task<bool> MigrateTo(string server);
        Task<bool> MigrateTo(ServerContract server);

        Task TrySendAliveReq();
        Task TryRecvAliveAck();
    }
}
