using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionLevelTemplate : ITemplate
    {
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

        // public short IncCDr { get; }
        // public short IncMAMr { get; }
        // public short IncSkill { get; }
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

        public ItemOptionLevelTemplate(int id, IDataProperty property)
        {
            ID = id;

            Prob = property.Resolve<int>("prop") ?? 0;
            Time = property.Resolve<int>("time") ?? 0;

            IncSTR = property.Resolve<short>("incSTR") ?? 0;
            IncDEX = property.Resolve<short>("incDEX") ?? 0;
            IncINT = property.Resolve<short>("incINT") ?? 0;
            IncLUK = property.Resolve<short>("incLUK") ?? 0;
            IncHP = property.Resolve<int>("incHP") ?? 0;
            IncMP = property.Resolve<int>("incMP") ?? 0;
            IncACC = property.Resolve<short>("incACC") ?? 0;
            IncEVA = property.Resolve<short>("incEVA") ?? 0;
            IncSpeed = property.Resolve<short>("incSpeed") ?? 0;
            IncJump = property.Resolve<short>("incJump") ?? 0;
            IncMaxHP = property.Resolve<short>("incMHP") ?? 0;
            IncMaxMP = property.Resolve<short>("incMMP") ?? 0;
            IncPAD = property.Resolve<short>("incPAD") ?? 0;
            IncMAD = property.Resolve<short>("incMAD") ?? 0;
            IncPDD = property.Resolve<short>("incPDD") ?? 0;
            IncMDD = property.Resolve<short>("incMDD") ?? 0;

            IncSTRr = property.Resolve<short>("incSTRr") ?? 0;
            IncDEXr = property.Resolve<short>("incDEXr") ?? 0;
            IncINTr = property.Resolve<short>("incINTr") ?? 0;
            IncLUKr = property.Resolve<short>("incLUKr") ?? 0;
            IncACCr = property.Resolve<short>("incACCr") ?? 0;
            IncEVAr = property.Resolve<short>("incEVAr") ?? 0;
            IncMaxHPr = property.Resolve<short>("incMHPr") ?? 0;
            IncMaxMPr = property.Resolve<short>("incMMPr") ?? 0;
            IncPADr = property.Resolve<short>("incPADr") ?? 0;
            IncMADr = property.Resolve<short>("incMADr") ?? 0;
            IncPDDr = property.Resolve<short>("incPDDr") ?? 0;
            IncMDDr = property.Resolve<short>("incMDDr") ?? 0;

            IncCr = property.Resolve<short>("incCr") ?? 0;
            IncAllSkill = property.Resolve<short>("incAllskill") ?? 0;
            RecoveryHP = property.Resolve<short>("RecoveryHP") ?? 0;
            RecoveryMP = property.Resolve<short>("RecoveryMP") ?? 0;
            RecoveryUP = property.Resolve<short>("RecoveryUP") ?? 0;
            MPConReduce = property.Resolve<short>("mpconReduce") ?? 0;
            MPConRestore = property.Resolve<short>("mpRestore") ?? 0;
            IgnoreTargetDEF = property.Resolve<short>("ignoreTargetDEF") ?? 0;
            IgnoreDAM = property.Resolve<short>("ignoreDAM") ?? 0;
            IgnoreDAMr = property.Resolve<short>("ignoreDAMr") ?? 0;
            IncDAMr = property.Resolve<short>("incDAMr") ?? 0;
            DAMReflect = property.Resolve<short>("DAMreflect") ?? 0;
            AttackType = property.Resolve<short>("attackType") ?? 0;
            IncMesoProb = property.Resolve<int>("incMesoProp") ?? 0;
            IncRewardProb = property.Resolve<int>("incRewardProp") ?? 0;
            Level = property.Resolve<short>("level") ?? 0;
            Boss = property.Resolve<short>("boss") ?? 0;
        }
    }
}