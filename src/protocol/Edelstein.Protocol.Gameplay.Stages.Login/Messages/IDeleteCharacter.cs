using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface IDeleteCharacter : IStageUserMessage<ILoginStageUser>
{
    string SPW { get; }
    int CharacterID { get; }
}
