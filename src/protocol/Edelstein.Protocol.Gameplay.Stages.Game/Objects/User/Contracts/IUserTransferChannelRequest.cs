namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserTransferChannelRequest : IFieldUserContract
{
    int ChannelID { get; }
}
