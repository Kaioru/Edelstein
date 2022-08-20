using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IDeleteCharacter : IStageUserContract<ILoginStageUser>
{
    string SPW { get; }
    int CharacterID { get; }
}
