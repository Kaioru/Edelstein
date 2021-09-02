using Edelstein.Common.Gameplay.Stages.Login;

namespace Edelstein.App.Login
{
    public record ProgramConfig : LoginStageConfig
    {
        public short Version { get; init; } = 95;
        public string Patch { get; init; } = "1";
        public byte Locale { get; init; } = 8;
    }
}
