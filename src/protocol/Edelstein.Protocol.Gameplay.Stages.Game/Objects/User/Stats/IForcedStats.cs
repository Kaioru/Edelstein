using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface IForcedStats
    {
        short? STR { get; }
        short? DEX { get; }
        short? INT { get; }
        short? LUK { get; }
        short? PAD { get; }
        short? PDD { get; }
        short? MAD { get; }
        short? MDD { get; }
        short? ACC { get; }
        short? EVA { get; }
        byte? Speed { get; }
        byte? Jump { get; }
        byte? SpeedMax { get; }

        Task Reset();
    }
}
