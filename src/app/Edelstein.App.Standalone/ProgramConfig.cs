using System.Collections.Generic;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Login;

namespace Edelstein.App.Standalone
{
    public record ProgramConfig
    {
        public ICollection<LoginStageConfig> LoginStages { get; init; }
        public ICollection<GameStageConfig> GameStages { get; init; }

        public short Version { get; init; }
        public string Patch { get; init; }
        public byte Locale { get; init; }

        public string Database { get; init; }
        public string DataPath { get; init; }
    }
}
