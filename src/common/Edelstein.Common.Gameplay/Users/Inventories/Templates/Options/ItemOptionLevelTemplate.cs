using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates.Options
{
    public record ItemOptionLevelTemplate : ITemplate
    {
        public int ID { get; init; }

        public int Prob { get; init; }
        public int Time { get; init; }

        public short IncSTR { get; init; }
        public short IncDEX { get; init; }
        public short IncINT { get; init; }
        public short IncLUK { get; init; }
        public int IncHP { get; init; }
        public int IncMP { get; init; }
        public short IncACC { get; init; }
        public short IncEVA { get; init; }
        public short IncSpeed { get; init; }
        public short IncJump { get; init; }
        public int IncMaxHP { get; init; }
        public int IncMaxMP { get; init; }
        public short IncPAD { get; init; }
        public short IncMAD { get; init; }
        public short IncPDD { get; init; }
        public short IncMDD { get; init; }

        public short IncSTRr { get; init; }
        public short IncDEXr { get; init; }
        public short IncINTr { get; init; }
        public short IncLUKr { get; init; }
        public int IncMaxHPr { get; init; }
        public int IncMaxMPr { get; init; }
        public short IncACCr { get; init; }
        public short IncEVAr { get; init; }
        public short IncPADr { get; init; }
        public short IncMADr { get; init; }
        public short IncPDDr { get; init; }
        public short IncMDDr { get; init; }

        public short IncCr { get; init; }

        // public short IncCDr { get; init; }
        // public short IncMAMr { get; init; }
        // public short IncSkill { get; init; }
        public short IncAllSkill { get; init; }
        public short RecoveryHP { get; init; }
        public short RecoveryMP { get; init; }
        public short RecoveryUP { get; init; }
        public short MPConReduce { get; init; }
        public short MPConRestore { get; init; }
        public short IgnoreTargetDEF { get; init; }
        public short IgnoreDAM { get; init; }
        public short IgnoreDAMr { get; init; }
        public short IncDAMr { get; init; }
        public short DAMReflect { get; init; }
        public short AttackType { get; init; }
        public int IncMesoProb { get; init; }
        public int IncRewardProb { get; init; }
        public short Level { get; init; }
        public short Boss { get; init; }

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
