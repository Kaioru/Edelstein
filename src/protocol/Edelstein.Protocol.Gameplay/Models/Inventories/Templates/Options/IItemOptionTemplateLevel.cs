namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;

public interface IItemOptionTemplateLevel
{
    int ID { get; }

    int Prob { get; }
    int Time { get; }

    short IncSTR { get; }
    short IncDEX { get; }
    short IncINT { get; }
    short IncLUK { get; }
    int IncHP { get; }
    int IncMP { get; }
    short IncACC { get; }
    short IncEVA { get; }
    short IncSpeed { get; }
    short IncJump { get; }
    int IncMaxHP { get; }
    int IncMaxMP { get; }
    short IncPAD { get; }
    short IncMAD { get; }
    short IncPDD { get; }
    short IncMDD { get; }

    short IncSTRr { get; }
    short IncDEXr { get; }
    short IncINTr { get; }
    short IncLUKr { get; }
    int IncMaxHPr { get; }
    int IncMaxMPr { get; }
    short IncACCr { get; }
    short IncEVAr { get; }
    short IncPADr { get; }
    short IncMADr { get; }
    short IncPDDr { get; }
    short IncMDDr { get; }
    short IncCr { get; }
    
    short IncAllSkill { get; }
    
    short RecoveryHP { get; }
    short RecoveryMP { get; }
    short RecoveryUP { get; }
    short MPConReduce { get; }
    short MPConRestore { get; }
    
    short IgnoreTargetDEF { get; }
    short IgnoreDAM { get; }
    short IgnoreDAMr { get; }
    short IncDAMr { get; }
    short DAMReflect { get; }
    short AttackType { get; }
    int IncMesoProb { get; }
    int IncRewardProb { get; }
    
    short Level { get; }
    short Boss { get; }
    
    // Emotion
}
