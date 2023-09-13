﻿using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Sets;

public record ItemSetTemplateEffect : IItemSetTemplateEffect
{
    public ItemSetTemplateEffect(int id, IDataProperty property)
    {
        IncSTR = property.Resolve<short>("incSTR") ?? 0;
        IncDEX = property.Resolve<short>("incDEX") ?? 0;
        IncINT = property.Resolve<short>("incINT") ?? 0;
        IncLUK = property.Resolve<short>("incLUK") ?? 0;
        IncMaxHP = property.Resolve<short>("incMHP") ?? 0;
        IncMaxMP = property.Resolve<short>("incMMP") ?? 0;
        IncPAD = property.Resolve<short>("incPAD") ?? 0;
        IncMAD = property.Resolve<short>("incMAD") ?? 0;
        IncPDD = property.Resolve<short>("incPDD") ?? 0;
        IncMDD = property.Resolve<short>("incMDD") ?? 0;
        IncACC = property.Resolve<short>("incACC") ?? 0;
        IncEVA = property.Resolve<short>("incEVA") ?? 0;
        IncCraft = property.Resolve<short>("incCraft") ?? 0;
        IncSpeed = property.Resolve<short>("incSpeed") ?? 0;
        IncJump = property.Resolve<short>("incJump") ?? 0;
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
