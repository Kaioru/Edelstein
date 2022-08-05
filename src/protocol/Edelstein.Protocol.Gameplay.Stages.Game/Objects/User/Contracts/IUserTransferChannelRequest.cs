namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserTransferChannelRequest : IFieldUserMessage
{
    int ChannelID { get; }
}
