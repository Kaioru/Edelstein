using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ICheckDuplicatedID : IStageUserContract<ILoginStageUser>
{
    string Name { get; }
}
