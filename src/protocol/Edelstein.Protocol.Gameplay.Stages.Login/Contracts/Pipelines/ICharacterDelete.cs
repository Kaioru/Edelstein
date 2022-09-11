namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICharacterDelete : ILoginStageUserContract
{
    string SPW { get; }
    int CharacterID { get; }
}
