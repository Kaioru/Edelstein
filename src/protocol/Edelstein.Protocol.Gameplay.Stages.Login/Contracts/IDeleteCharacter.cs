using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface IDeleteCharacter : IStageUserContract<ILoginStageUser>
{
    string SPW { get; }
    int CharacterID { get; }
}
