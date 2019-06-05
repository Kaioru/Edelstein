namespace Edelstein.Provider.Templates.Item.Consume
{
    public class StateChangeItemTemplate : ItemBundleTemplate
    {
        public int HP { get; set; }
        public int MP { get; set; }
        public int HPr { get; set; }
        public int MPr { get; set; }

        public bool NoCancelMouse { get; set; }

        public short PAD { get; set; }
        public short PDD { get; set; }
        public short MAD { get; set; }
        public short MDD { get; set; }
        public short ACC { get; set; }
        public short EVA { get; set; }
        public short Craft { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public short Morph { get; set; }
        public int Time { get; set; }

        public StateChangeItemTemplate(int id, IDataProperty info, IDataProperty spec) : base(id, info)
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