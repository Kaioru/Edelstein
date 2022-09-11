using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;

namespace Edelstein.Daemon.Server.Configs;

public record ProgramConfigStageChat : AbstractProgramConfigStage, IChatContextOptions
{
    public int WorldID { get; set; }
}
