using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface IDeleteCharacter : IStageUserMessage<ILoginStageUser>
{
    string SPW { get; }
    int CharacterID { get; }
}
