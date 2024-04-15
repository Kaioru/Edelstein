using Edelstein.Protocol.Gameplay.Contexts;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public interface ILoginContextPipelines : IStageContextPipelines<ILoginStageUser, ILoginStageSystem>
{
}
