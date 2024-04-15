using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Gameplay.Handling;

public interface IPacketHandler<in TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    short Operation { get; }

    bool Check(TStageUser user);
    Task Handle(TStageUser user, IPacketReader reader);
}
