namespace Edelstein.Protocol.Gameplay.Stages.Game.Messages;

public interface IUserTransferChannelRequest : IFieldUserMessage
{
    int ChannelID { get; }
}
