using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemEffectTemplate : ITemplate
    {
        public int ID { get; set; }
        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public short IncMaxHP { get; set; }
        public short IncMaxMP { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncCraft { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }

        public static SetItemEffectTemplate Parse(int id, IDataProperty p)
        {
            return new SetItemEffectTemplate
            {
                ID = id,
                IncSTR = p.Resolve<short>("incSTR") ?? 0,
                IncDEX = p.Resolve<short>("incDEX") ?? 0,
                IncINT = p.Resolve<short>("incINT") ?? 0,
                IncLUK = p.Resolve<short>("incLUK") ?? 0,
                IncMaxHP = p.Resolve<short>("incMHP") ?? 0,
                IncMaxMP = p.Resolve<short>("incMMP") ?? 0,
                IncPAD = p.Resolve<short>("incPAD") ?? 0,
                IncMAD = p.Resolve<short>("incMAD") ?? 0,
                IncPDD = p.Resolve<short>("incPDD") ?? 0,
                IncMDD = p.Resolve<short>("incMDD") ?? 0,
                IncACC = p.Resolve<short>("incACC") ?? 0,
                IncEVA = p.Resolve<short>("incEVA") ?? 0,
                IncCraft = p.Resolve<short>("incCraft") ?? 0,
                IncSpeed = p.Resolve<short>("incSpeed") ?? 0,
                IncJump = p.Resolve<short>("incJump") ?? 0,
            };
        }
    }
}