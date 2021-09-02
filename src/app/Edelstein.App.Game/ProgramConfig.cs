using Edelstein.Common.Gameplay.Stages.Game;

namespace Edelstein.App.Game
{
    public record ProgramConfig : GameStageConfig
    {
        public short Version { get; init; } = 95;
        public string Patch { get; init; } = "1";
        public byte Locale { get; init; } = 8;
    }
}
