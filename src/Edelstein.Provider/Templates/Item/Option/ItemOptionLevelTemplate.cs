namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionLevelTemplate : ITemplate
    {
        public int ID { get; }

        public int Prob { get; set; }
        public int Time { get; set; }

        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public int IncHP { get; set; }
        public int IncMP { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }

        public short IncSTRr { get; set; }
        public short IncDEXr { get; set; }
        public short IncINTr { get; set; }
        public short IncLUKr { get; set; }
        public int IncMaxHPr { get; set; }
        public int IncMaxMPr { get; set; }
        public short IncACCr { get; set; }
        public short IncEVAr { get; set; }
        public short IncPADr { get; set; }
        public short IncMADr { get; set; }
        public short IncPDDr { get; set; }
        public short IncMDDr { get; set; }

        public short IncCr { get; set; }

        // public short IncCDr { get; set; }
        // public short IncMAMr { get; set; }
        // public short IncSkill { get; set; }
        public short IncAllSkill { get; set; }
        public short RecoveryHP { get; set; }
        public short RecoveryMP { get; set; }
        public short RecoveryUP { get; set; }
        public short MPConReduce { get; set; }
        public short MPConRestore { get; set; }
        public short IgnoreTargetDEF { get; set; }
        public short IgnoreDAM { get; set; }
        public short IgnoreDAMr { get; set; }
        public short IncDAMr { get; set; }
        public short DAMReflect { get; set; }
        public short AttackType { get; set; }
        public int IncMesoProb { get; set; }
        public int IncRewardProb { get; set; }
        public short Level { get; set; }
        public short Boss { get; set; }

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