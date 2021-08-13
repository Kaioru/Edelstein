using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ICalculatedStats
    {
        public int STR { get; }
        public int DEX { get; }
        public int INT { get; }
        public int LUK { get; }

        public int MaxHP { get; }
        public int MaxMP { get; }

        public int PAD { get; }
        public int PDD { get; }
        public int MAD { get; }
        public int MDD { get; }
        public int ACC { get; }
        public int EVA { get; }
        public int Craft { get; }
        public int Speed { get; }
        public int Jump { get; }

        public int STRr { get; }
        public int DEXr { get; }
        public int INTr { get; }
        public int LUKr { get; }

        public int MaxHPr { get; }
        public int MaxMPr { get; }

        public int PADr { get; }
        public int PDDr { get; }
        public int MADr { get; }
        public int MDDr { get; }
        public int ACCr { get; }
        public int EVAr { get; }

        Task Calculate();
    }
}
