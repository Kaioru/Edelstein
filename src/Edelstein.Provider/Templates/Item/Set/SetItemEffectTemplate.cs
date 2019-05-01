namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemEffectTemplate : ITemplate
    {
        public int ID { get; }

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

        public SetItemEffectTemplate(int id, IDataProperty property)
        {
            ID = id;

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
    }
}