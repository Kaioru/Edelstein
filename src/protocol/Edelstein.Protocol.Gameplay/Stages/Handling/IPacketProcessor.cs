using System.Threading.Tasks;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Handling
{
    public interface IPacketProcessor<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        Task Process(TUser user, IPacketReader packet);
    }
}
