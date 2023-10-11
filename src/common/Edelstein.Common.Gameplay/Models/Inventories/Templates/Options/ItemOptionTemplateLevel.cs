using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates.Options;

public record ItemOptionTemplateLevel : IItemOptionTemplateLevel
{
    public ItemOptionTemplateLevel(int id, IDataNode node)
    {
        ID = id;

        Prob = node.ResolveInt("prop") ?? 0;
        Time = node.ResolveInt("time") ?? 0;

        IncSTR = node.ResolveShort("incSTR") ?? 0;
        IncDEX = node.ResolveShort("incDEX") ?? 0;
        IncINT = node.ResolveShort("incINT") ?? 0;
        IncLUK = node.ResolveShort("incLUK") ?? 0;
        IncHP = node.ResolveInt("incHP") ?? 0;
        IncMP = node.ResolveInt("incMP") ?? 0;
        IncACC = node.ResolveShort("incACC") ?? 0;
        IncEVA = node.ResolveShort("incEVA") ?? 0;
        IncSpeed = node.ResolveShort("incSpeed") ?? 0;
        IncJump = node.ResolveShort("incJump") ?? 0;
        IncMaxHP = node.ResolveShort("incMHP") ?? 0;
        IncMaxMP = node.ResolveShort("incMMP") ?? 0;
        IncPAD = node.ResolveShort("incPAD") ?? 0;
        IncMAD = node.ResolveShort("incMAD") ?? 0;
        IncPDD = node.ResolveShort("incPDD") ?? 0;
        IncMDD = node.ResolveShort("incMDD") ?? 0;

        IncSTRr = node.ResolveShort("incSTRr") ?? 0;
        IncDEXr = node.ResolveShort("incDEXr") ?? 0;
        IncINTr = node.ResolveShort("incINTr") ?? 0;
        IncLUKr = node.ResolveShort("incLUKr") ?? 0;
        IncACCr = node.ResolveShort("incACCr") ?? 0;
        IncEVAr = node.ResolveShort("incEVAr") ?? 0;
        IncMaxHPr = node.ResolveShort("incMHPr") ?? 0;
        IncMaxMPr = node.ResolveShort("incMMPr") ?? 0;
        IncPADr = node.ResolveShort("incPADr") ?? 0;
        IncMADr = node.ResolveShort("incMADr") ?? 0;
        IncPDDr = node.ResolveShort("incPDDr") ?? 0;
        IncMDDr = node.ResolveShort("incMDDr") ?? 0;

        IncCr = node.ResolveShort("incCr") ?? 0;
        
        IncAllSkill = node.ResolveShort("incAllskill") ?? 0;
        
        RecoveryHP = node.ResolveShort("RecoveryHP") ?? 0;
        RecoveryMP = node.ResolveShort("RecoveryMP") ?? 0;
        RecoveryUP = node.ResolveShort("RecoveryUP") ?? 0;
        MPConReduce = node.ResolveShort("mpconReduce") ?? 0;
        MPConRestore = node.ResolveShort("mpRestore") ?? 0;
        IgnoreTargetDEF = node.ResolveShort("ignoreTargetDEF") ?? 0;
        IgnoreDAM = node.ResolveShort("ignoreDAM") ?? 0;
        IgnoreDAMr = node.ResolveShort("ignoreDAMr") ?? 0;
        IncDAMr = node.ResolveShort("incDAMr") ?? 0;
        DAMReflect = node.ResolveShort("DAMreflect") ?? 0;
        AttackType = node.ResolveShort("attackType") ?? 0;
        IncMesoProb = node.ResolveInt("incMesoProp") ?? 0;
        IncRewardProb = node.ResolveInt("incRewardProp") ?? 0;
        
        Level = node.ResolveShort("level") ?? 0;
        Boss = node.ResolveShort("boss") ?? 0;
    }
    
    public int ID { get; }
    
    public int Prob { get; }
    public int Time { get; }
    
    public short IncSTR { get; }
    public short IncDEX { get; }
    public short IncINT { get; }
    public short IncLUK { get; }
    public int IncHP { get; }
    public int IncMP { get; }
    public short IncACC { get; }
    public short IncEVA { get; }
    public short IncSpeed { get; }
    public short IncJump { get; }
    public int IncMaxHP { get; }
    public int IncMaxMP { get; }
    public short IncPAD { get; }
    public short IncMAD { get; }
    public short IncPDD { get; }
    public short IncMDD { get; }
    
    public short IncSTRr { get; }
    public short IncDEXr { get; }
    public short IncINTr { get; }
    public short IncLUKr { get; }
    public int IncMaxHPr { get; }
    public int IncMaxMPr { get; }
    public short IncACCr { get; }
    public short IncEVAr { get; }
    public short IncPADr { get; }
    public short IncMADr { get; }
    public short IncPDDr { get; }
    public short IncMDDr { get; }
    public short IncCr { get; }
    
    public short IncAllSkill { get; }
    
    public short RecoveryHP { get; }
    public short RecoveryMP { get; }
    public short RecoveryUP { get; }
    public short MPConReduce { get; }
    public short MPConRestore { get; }
    public short IgnoreTargetDEF { get; }
    public short IgnoreDAM { get; }
    public short IgnoreDAMr { get; }
    public short IncDAMr { get; }
    public short DAMReflect { get; }
    public short AttackType { get; }
    public int IncMesoProb { get; }
    public int IncRewardProb { get; }
    
    public short Level { get; }
    public short Boss { get; }
}
