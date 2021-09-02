using System.Collections.Generic;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Login;

namespace Edelstein.App.Standalone
{
    public record ProgramConfig
    {
        public ICollection<LoginStageConfig> LoginStages { get; init; }
        public ICollection<GameStageConfig> GameStages { get; init; }

        public short Version { get; init; } = 95;
        public string Patch { get; init; } = "1";
        public byte Locale { get; init; } = 8;
    }
}
