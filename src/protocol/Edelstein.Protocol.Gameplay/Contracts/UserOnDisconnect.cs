namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnDisconnect<TStageUser, TStageSystem>(
    TStageUser User
) 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>;
