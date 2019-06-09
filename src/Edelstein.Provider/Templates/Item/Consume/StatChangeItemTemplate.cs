using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Item.Consume
{
    public class StatChangeItemTemplate : ItemBundleTemplate
    {
        public int HP { get; }
        public int MP { get; }
        public int HPr { get; }
        public int MPr { get; }

        public bool NoCancelMouse { get; }

        public short PAD { get; }
        public short PDD { get; }
        public short MAD { get; }
        public short MDD { get; }
        public short ACC { get; }
        public short EVA { get; }
        public short Craft { get; }
        public short Speed { get; }
        public short Jump { get; }

        public short Morph { get; }
        public int Time { get; }

        public StatChangeItemTemplate(int id, IDataProperty info, IDataProperty spec) : base(id, info)
        {
            HP = spec.Resolve<short>("hp") ?? 0;
            MP = spec.Resolve<short>("mp") ?? 0;
            HPr = spec.Resolve<short>("hpR") ?? 0;
            MPr = spec.Resolve<short>("mpR") ?? 0;

            NoCancelMouse = info.Resolve<bool>("noCancelMouse") ?? false;

            PAD = spec.Resolve<short>("pad") ?? 0;
            PDD = spec.Resolve<short>("pdd") ?? 0;
            MAD = spec.Resolve<short>("mad") ?? 0;
            MDD = spec.Resolve<short>("mdd") ?? 0;
            ACC = spec.Resolve<short>("acc") ?? 0;
            EVA = spec.Resolve<short>("eva") ?? 0;
            Craft = spec.Resolve<short>("craft") ?? 0;
            Speed = spec.Resolve<short>("speed") ?? 0;
            Jump = spec.Resolve<short>("jump") ?? 0;

            Morph = spec.Resolve<short>("morph") ?? 0;
            Time = spec.Resolve<int>("time") ?? 0;
        }
    }
}