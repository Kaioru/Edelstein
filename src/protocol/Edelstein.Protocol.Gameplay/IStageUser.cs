using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser<TStage, TStageUser> : IIdentifiable<int>, IAdapter
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
}
