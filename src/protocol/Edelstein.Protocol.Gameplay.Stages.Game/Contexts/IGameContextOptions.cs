using Edelstein.Protocol.Gameplay.Stages.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextOptions : IStageContextOptions
{
    int WorldID { get; }
    int ChannelID { get; }
    bool IsAdultChannel { get; set; }
}
