namespace Edelstein.Protocol.Gameplay.Game.Combat.Stats;

public interface IStatModifier
{
    int Min { get; set; }
    int Max { get; set; }
    
    int IncBase { get; set; }
    int IncRate { get; set; }
    int IncFlat { get; set; }
    
    int Apply(int value);
}
