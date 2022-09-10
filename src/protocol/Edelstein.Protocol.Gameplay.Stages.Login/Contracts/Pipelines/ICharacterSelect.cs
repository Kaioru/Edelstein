namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICharacterSelect : ILoginStageUserContract
{
    int CharacterID { get; }
}
