using Edelstein.Protocol.Gameplay.Login.Contexts;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystem : IStageSystem<ILoginStageSystem, ILoginStageSystemUser>
{
    ILoginStageSystemOptions Options { get; }
    LoginContext Context { get; }
}
