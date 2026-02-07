using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IGameStageOptions : IIdentifiable<string>
{
    int WorldID { get; }
    int ChannelID { get; }

    double ExpRate { get; }
    double MesoRate { get; }
    double DropRate { get; }

    bool IsAdultChannel { get; }
}
