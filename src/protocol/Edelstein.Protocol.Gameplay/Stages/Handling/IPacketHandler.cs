using System.Threading.Tasks;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Handling
{
    public interface IPacketHandler<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        short Operation { get; }

        Task Handle(TUser user, IPacketReader packet);
    }
}
