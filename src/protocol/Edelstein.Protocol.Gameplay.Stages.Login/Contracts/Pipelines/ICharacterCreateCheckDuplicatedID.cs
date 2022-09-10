namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICharacterCreateCheckDuplicatedID : ILoginStageUserContract
{
    string Name { get; }
}
