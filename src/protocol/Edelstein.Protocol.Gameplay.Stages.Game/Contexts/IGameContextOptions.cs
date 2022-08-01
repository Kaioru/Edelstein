namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextOptions
{
    int WorldID { get; }
    int ChannelID { get; }
    bool IsAdultChannel { get; set; }
}
