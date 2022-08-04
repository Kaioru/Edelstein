using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats;

public readonly struct CalculatedSeeds : ICalculatedSeeds
{
    public uint Seed1 { get; init; }
    public uint Seed2 { get; init; }
    public uint Seed3 { get; init; }

    public CalculatedSeeds(uint seed1, uint seed2, uint seed3)
    {
        Seed1 = seed1;
        Seed2 = seed2;
        Seed3 = seed3;
    }
}
