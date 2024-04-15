namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystem : IStageSystem<ILoginStageUser, ILoginStageSystem>
{
    ILoginStageSystemOptions Options { get; }
}
