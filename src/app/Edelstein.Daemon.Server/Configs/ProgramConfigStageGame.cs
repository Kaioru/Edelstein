using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

namespace Edelstein.Daemon.Server.Configs;

public record ProgramConfigStageGame : AbstractProgramConfigStage, IGameContextOptions
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }
    public bool IsAdultChannel { get; set; }
}
