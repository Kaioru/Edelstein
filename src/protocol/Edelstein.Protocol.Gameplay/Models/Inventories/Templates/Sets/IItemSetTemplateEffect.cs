namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

public interface IItemSetTemplateEffect
{
    short IncSTR { get; }
    short IncDEX { get; }
    short IncINT { get; }
    short IncLUK { get; }
    short IncMaxHP { get; }
    short IncMaxMP { get; }
    short IncPAD { get; }
    short IncMAD { get; }
    short IncPDD { get; }
    short IncMDD { get; }
    short IncACC { get; }
    short IncEVA { get; }
    short IncCraft { get; }
    short IncSpeed { get; }
    short IncJump { get; }
}
