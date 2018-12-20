using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionLevelTemplate : ITemplate
    {
        public int ID { get; set; }
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

        public static ItemOptionLevelTemplate Parse(int id, IDataProperty property)
        {
            var t = new ItemOptionLevelTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Prob = p.Resolve<int>("prop") ?? 0;
                t.Time = p.Resolve<int>("time") ?? 0;
                t.IncSTR = p.Resolve<short>("incSTR") ?? 0;
                t.IncDEX = p.Resolve<short>("incDEX") ?? 0;
                t.IncINT = p.Resolve<short>("incINT") ?? 0;
                t.IncLUK = p.Resolve<short>("incLUK") ?? 0;
                t.IncHP = p.Resolve<int>("incHP") ?? 0;
                t.IncMP = p.Resolve<int>("incMP") ?? 0;
                t.IncACC = p.Resolve<short>("incACC") ?? 0;
                t.IncEVA = p.Resolve<short>("incEVA") ?? 0;
                t.IncSpeed = p.Resolve<short>("incSpeed") ?? 0;
                t.IncJump = p.Resolve<short>("incJump") ?? 0;
                t.IncMaxHP = p.Resolve<short>("incMHP") ?? 0;
                t.IncMaxMP = p.Resolve<short>("incMMP") ?? 0;
                t.IncPAD = p.Resolve<short>("incPAD") ?? 0;
                t.IncMAD = p.Resolve<short>("incMAD") ?? 0;
                t.IncPDD = p.Resolve<short>("incPDD") ?? 0;
                t.IncMDD = p.Resolve<short>("incMDD") ?? 0;

                t.IncSTRr = p.Resolve<short>("incSTRr") ?? 0;
                t.IncDEXr = p.Resolve<short>("incDEXr") ?? 0;
                t.IncINTr = p.Resolve<short>("incINTr") ?? 0;
                t.IncLUKr = p.Resolve<short>("incLUKr") ?? 0;
                t.IncACCr = p.Resolve<short>("incACCr") ?? 0;
                t.IncEVAr = p.Resolve<short>("incEVAr") ?? 0;
                t.IncMaxHPr = p.Resolve<short>("incMHPr") ?? 0;
                t.IncMaxMPr = p.Resolve<short>("incMMPr") ?? 0;
                t.IncPADr = p.Resolve<short>("incPADr") ?? 0;
                t.IncMADr = p.Resolve<short>("incMADr") ?? 0;
                t.IncPDDr = p.Resolve<short>("incPDDr") ?? 0;
                t.IncMDDr = p.Resolve<short>("incMDDr") ?? 0;

                t.IncCr = p.Resolve<short>("incCr") ?? 0;
                t.IncAllSkill = p.Resolve<short>("incAllskill") ?? 0;
                t.RecoveryHP = p.Resolve<short>("RecoveryHP") ?? 0;
                t.RecoveryMP = p.Resolve<short>("RecoveryMP") ?? 0;
                t.RecoveryUP = p.Resolve<short>("RecoveryUP") ?? 0;
                t.MPConReduce = p.Resolve<short>("mpconReduce") ?? 0;
                t.MPConRestore = p.Resolve<short>("mpRestore") ?? 0;
                t.IgnoreTargetDEF = p.Resolve<short>("ignoreTargetDEF") ?? 0;
                t.IgnoreDAM = p.Resolve<short>("ignoreDAM") ?? 0;
                t.IgnoreDAMr = p.Resolve<short>("ignoreDAMr") ?? 0;
                t.IncDAMr = p.Resolve<short>("incDAMr") ?? 0;
                t.DAMReflect = p.Resolve<short>("DAMreflect") ?? 0;
                t.AttackType = p.Resolve<short>("attackType") ?? 0;
                t.IncMesoProb = p.Resolve<int>("incMesoProp") ?? 0;
                t.IncRewardProb = p.Resolve<int>("incRewardProp") ?? 0;
                t.Level = p.Resolve<short>("level") ?? 0;
                t.Boss = p.Resolve<short>("boss") ?? 0;
            });
            return t;
        }
    }
}