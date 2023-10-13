using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates.Sets;

public record ItemSetTemplateEffect : IItemSetTemplateEffect
{
    public ItemSetTemplateEffect(int id, IDataNode node)
    {
        IncSTR = node.ResolveShort("incSTR") ?? 0;
        IncDEX = node.ResolveShort("incDEX") ?? 0;
        IncINT = node.ResolveShort("incINT") ?? 0;
        IncLUK = node.ResolveShort("incLUK") ?? 0;
        IncMaxHP = node.ResolveShort("incMHP") ?? 0;
        IncMaxMP = node.ResolveShort("incMMP") ?? 0;
        IncPAD = node.ResolveShort("incPAD") ?? 0;
        IncMAD = node.ResolveShort("incMAD") ?? 0;
        IncPDD = node.ResolveShort("incPDD") ?? 0;
        IncMDD = node.ResolveShort("incMDD") ?? 0;
        IncACC = node.ResolveShort("incACC") ?? 0;
        IncEVA = node.ResolveShort("incEVA") ?? 0;
        IncCraft = node.ResolveShort("incCraft") ?? 0;
        IncSpeed = node.ResolveShort("incSpeed") ?? 0;
        IncJump = node.ResolveShort("incJump") ?? 0;
    }
    
    public short IncSTR { get; }
    public short IncDEX { get; }
    public short IncINT { get; }
    public short IncLUK { get; }
    public short IncMaxHP { get; }
    public short IncMaxMP { get; }
    public short IncPAD { get; }
    public short IncMAD { get; }
    public short IncPDD { get; }
    public short IncMDD { get; }
    public short IncACC { get; }
    public short IncEVA { get; }
    public short IncCraft { get; }
    public short IncSpeed { get; }
    public short IncJump { get; }
}
