using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Handling
{
    public interface IPacketHandler<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        short Operation { get; }

        Task<bool> Check(TUser user);
        Task Handle(TUser user, IPacketReader packet);
    }
}
