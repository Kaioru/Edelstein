﻿namespace Edelstein.Application.Server.Configs;

public class ProgramConfig
{
    public ICollection<ProgramConfigStageLogin> LoginStages { get; set; } = new List<ProgramConfigStageLogin>();

    public bool MigrateDatabaseOnInit { get; set; } = false;
    public int TicksPerSecond { get; set; } = 4;
}
