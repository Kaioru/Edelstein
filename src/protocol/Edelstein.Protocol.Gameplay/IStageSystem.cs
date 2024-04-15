using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystem<TStageUser, TStageSystem> : 
    IRepositoryEntry<string>, 
    ISocketUserCreator<TStageUser>, 
    ISocketAdapter<TStageUser> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>;
