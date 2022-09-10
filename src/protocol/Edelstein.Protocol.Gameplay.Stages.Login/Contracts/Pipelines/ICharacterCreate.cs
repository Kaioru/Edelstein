namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICharacterCreate : ILoginStageUserContract
{
    string Name { get; }
    int Race { get; }
    short SubJob { get; }
    byte Gender { get; }
    byte Skin { get; }
    int[] Look { get; }
}
