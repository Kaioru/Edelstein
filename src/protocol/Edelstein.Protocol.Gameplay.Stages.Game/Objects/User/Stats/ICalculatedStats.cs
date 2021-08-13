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
        int ACC { get; }
        int EVA { get; }
        int Craft { get; }
        int Speed { get; }
        int Jump { get; }

        int STRr { get; }
        int DEXr { get; }
        int INTr { get; }
        int LUKr { get; }

        int MaxHPr { get; }
        int MaxMPr { get; }

        int PADr { get; }
        int PDDr { get; }
        int MADr { get; }
        int MDDr { get; }
        int ACCr { get; }
        int EVAr { get; }

        Task Calculate();
    }
}
