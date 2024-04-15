using System;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnException<TStageUser, TStageSystem>(
    TStageUser User,
    Exception Exception
) 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>;
