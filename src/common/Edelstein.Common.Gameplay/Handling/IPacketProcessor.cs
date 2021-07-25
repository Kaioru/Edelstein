using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Handling
{
    public interface IPacketProcessor<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        void Register(IPacketHandler<TStage, TUser> handler);
        void Deregister(IPacketHandler<TStage, TUser> handler);

        Task Process(TUser user, IPacketReader packet);
    }
}
