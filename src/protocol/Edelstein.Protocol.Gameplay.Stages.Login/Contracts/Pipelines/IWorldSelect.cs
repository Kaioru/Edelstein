namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IWorldSelect : ILoginStageUserContract
{
    byte WorldID { get; }
    byte ChannelID { get; }
}
