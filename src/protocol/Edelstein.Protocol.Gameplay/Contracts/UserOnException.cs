using System;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnException<TStageSystemUser, TStageSystem>(
    TStageSystemUser User,
    Exception Exception
)
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>;
