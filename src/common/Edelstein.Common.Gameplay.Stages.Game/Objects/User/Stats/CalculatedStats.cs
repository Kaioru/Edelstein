using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats;

public readonly struct CalculatedStats : ICalculatedStats
{
    public int STR { get; init; }
    public int DEX { get; init; }
    public int INT { get; init; }
    public int LUK { get; init; }
    public int MaxHP { get; init; }
    public int MaxMP { get; init; }
    public int PAD { get; init; }
    public int PDD { get; init; }
    public int MAD { get; init; }
    public int MDD { get; init; }
    public int PACC { get; init; }
    public int MACC { get; init; }
    public int PEVA { get; init; }
    public int MEVA { get; init; }

    public int Ar { get; init; }
    public int Er { get; init; }

    public int Craft { get; init; }
    public int Speed { get; init; }
    public int Jump { get; init; }

    public int Cr { get; init; }
    public int CDMin { get; init; }
    public int CDMax { get; init; }

    public int IMDr { get; init; }

    public int PDamR { get; init; }
    public int MDamR { get; init; }
    public int BossDamR { get; init; }
}
