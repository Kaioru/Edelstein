using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface ICalculatedMobStats
    {
        int MaxHP { get; }
        int MaxMP { get; }

        int PAD { get; }
        int PDR { get; }
        int MAD { get; }
        int MDR { get; }
        int ACC { get; }
        int EVA { get; }

        Task Calculate();
    }
}
