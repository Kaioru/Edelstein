using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ICalculatedStats
    {
        int STR { get; }
        int DEX { get; }
        int INT { get; }
        int LUK { get; }
        int MaxHP { get; }
        int MaxMP { get; }
        int PAD { get; }
        int PDD { get; }
        int MAD { get; }
        int MDD { get; }
        int PACC { get; }
        int MACC { get; }
        int PEVA { get; }
        int MEVA { get; }

        int Craft { get; }
        int Speed { get; }
        int Jump { get; }

        int Cr { get; }
        int CDMin { get; }
        int CDMax { get; }

        int IMDr { get; }

        int PDamR { get; }
        int MDamR { get; }
        int BossDamR { get; }

        int DamageMin { get; }
        int DamageMax { get; }

        Task Calculate();
    }
}
