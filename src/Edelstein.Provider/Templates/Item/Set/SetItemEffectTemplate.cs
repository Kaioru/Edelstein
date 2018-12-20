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

        public static SetItemEffectTemplate Parse(int id, IDataProperty property)
        {
            var t = new SetItemEffectTemplate {ID = id};

            property.Resolve(p =>
            {
                t.IncSTR = p.Resolve<short>("incSTR") ?? 0;
                t.IncDEX = p.Resolve<short>("incDEX") ?? 0;
                t.IncINT = p.Resolve<short>("incINT") ?? 0;
                t.IncLUK = p.Resolve<short>("incLUK") ?? 0;
                t.IncMaxHP = p.Resolve<short>("incMHP") ?? 0;
                t.IncMaxMP = p.Resolve<short>("incMMP") ?? 0;
                t.IncPAD = p.Resolve<short>("incPAD") ?? 0;
                t.IncMAD = p.Resolve<short>("incMAD") ?? 0;
                t.IncPDD = p.Resolve<short>("incPDD") ?? 0;
                t.IncMDD = p.Resolve<short>("incMDD") ?? 0;
                t.IncACC = p.Resolve<short>("incACC") ?? 0;
                t.IncEVA = p.Resolve<short>("incEVA") ?? 0;
                t.IncCraft = p.Resolve<short>("incCraft") ?? 0;
                t.IncSpeed = p.Resolve<short>("incSpeed") ?? 0;
                t.IncJump = p.Resolve<short>("incJump") ?? 0;
            });
            return t;
        }
    }
}