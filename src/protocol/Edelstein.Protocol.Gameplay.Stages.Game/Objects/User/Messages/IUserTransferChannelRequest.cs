namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;

public interface IUserTransferChannelRequest : IFieldUserMessage
{
    int ChannelID { get; }
}
