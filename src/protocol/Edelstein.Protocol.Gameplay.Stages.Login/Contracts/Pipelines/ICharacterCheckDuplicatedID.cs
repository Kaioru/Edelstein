namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICharacterCheckDuplicatedID : ILoginStageUserContract
{
    string Name { get; }
}
