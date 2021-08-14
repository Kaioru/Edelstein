using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Sets
{
    public record ItemSetEffectTemplate : ITemplate
    {
        public int ID { get; init; }

        public short IncSTR { get; init; }
        public short IncDEX { get; init; }
        public short IncINT { get; init; }
        public short IncLUK { get; init; }
        public short IncMaxHP { get; init; }
        public short IncMaxMP { get; init; }
        public short IncPAD { get; init; }
        public short IncMAD { get; init; }
        public short IncPDD { get; init; }
        public short IncMDD { get; init; }
        public short IncACC { get; init; }
        public short IncEVA { get; init; }
        public short IncCraft { get; init; }
        public short IncSpeed { get; init; }
        public short IncJump { get; init; }

        public ItemSetEffectTemplate(int id, IDataProperty property)
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
