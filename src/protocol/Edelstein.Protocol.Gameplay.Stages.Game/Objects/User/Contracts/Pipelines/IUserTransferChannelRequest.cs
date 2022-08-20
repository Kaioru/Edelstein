namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserTransferChannelRequest : IFieldUserContract
{
    int ChannelID { get; }
}
