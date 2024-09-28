namespace Edelstein.Protocol.Gameplay.Contracts;

public record SystemOnStart<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public required IStageSystem<TStageSystem, TStageSystemUser> System { get; init; }
}
