using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStageGame : ProgramConfigStage, IGameStageOptions
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }

    public double ExpRate { get; set; }
    public double MesoRate { get; set; }
    public double DropRate { get; set; }

    public bool IsAdultChannel { get; set; }
}
