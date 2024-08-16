using System;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnException<TStageSystem, TStageSystemUser>(
    TStageSystemUser User,
    Exception Exception
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>;
