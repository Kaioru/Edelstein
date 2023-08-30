namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IFieldMobStats
{
    int Level { get; }
    
    int PAD { get; }
    int PDD { get; }
    int PDR { get; }
    int MAD { get; }
    int MDD { get; }
    int MDR { get; }

    int ACC { get; }
    int EVA { get; }
}
