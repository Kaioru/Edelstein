using Edelstein.Protocol.Gameplay.Login.Contexts;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystem : IStageSystem<ILoginStageSystemUser, ILoginStageSystem>
{
    LoginContext Context { get; }
}
