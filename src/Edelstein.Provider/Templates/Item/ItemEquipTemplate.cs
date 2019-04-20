namespace Edelstein.Provider.Templates.Item
{
    public class ItemEquipTemplate : ItemTemplate
    {
        public short ReqSTR { get; set; }
        public short ReqDEX { get; set; }
        public short ReqINT { get; set; }
        public short ReqLUK { get; set; }
        public short ReqPOP { get; set; }
        public short ReqJob { get; set; }
        public byte ReqLevel { get; set; }

        public byte TUC { get; set; }
        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public int IncMaxHPr { get; set; }
        public int IncMaxMPr { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncCraft { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }

        // fs, swim, tamingmob
        // public int IUC { get; set; }
        // public byte MinGrade { get; set; }

        public bool OnlyEquip { get; set; }
        public bool TradeBlockEquip { get; set; }

        // nirPoison, nirIce, nirFire, nirLight, nirHoly
        // other random stuff

        public bool NotExtend { get; set; }
        public bool SharableOnce { get; set; }

        public byte AppliableKarmaType { get; set; }

        public int SetItemID { get; set; }

        public int Durability { get; set; }
        // public int EnchantCategory { get; set; }
        // public int Transform { get; set; }
        // public int IUCMax { get; set; }

        public ItemEquipTemplate(int id, IDataProperty info) : base(id, info)
        {
            ReqSTR = info.Resolve<short>("reqSTR") ?? 0;
            ReqDEX = info.Resolve<short>("reqDEX") ?? 0;
            ReqINT = info.Resolve<short>("reqINT") ?? 0;
            ReqLUK = info.Resolve<short>("reqLUK") ?? 0;
            ReqPOP = info.Resolve<short>("reqPOP") ?? 0;
            ReqJob = info.Resolve<short>("reqJob") ?? 0;
            ReqLevel = info.Resolve<byte>("reqLevel") ?? 0;

            TUC = info.Resolve<byte>("tuc") ?? 0;
            IncSTR = info.Resolve<short>("incSTR") ?? 0;
            IncDEX = info.Resolve<short>("incDEX") ?? 0;
            IncINT = info.Resolve<short>("incINT") ?? 0;
            IncLUK = info.Resolve<short>("incLUK") ?? 0;
            IncMaxHP = info.Resolve<int>("incMHP") ?? 0;
            IncMaxMP = info.Resolve<int>("incMMP") ?? 0;
            IncMaxHPr = info.Resolve<int>("incMHPr") ?? 0;
            IncMaxMPr = info.Resolve<int>("incMMPr") ?? 0;
            IncPAD = info.Resolve<short>("incPAD") ?? 0;
            IncMAD = info.Resolve<short>("incMAD") ?? 0;
            IncPDD = info.Resolve<short>("incPDD") ?? 0;
            IncMDD = info.Resolve<short>("incMDD") ?? 0;
            IncACC = info.Resolve<short>("incACC") ?? 0;
            IncEVA = info.Resolve<short>("incEVA") ?? 0;
            IncCraft = info.Resolve<short>("incCraft") ?? 0;
            IncSpeed = info.Resolve<short>("incSpeed") ?? 0;
            IncJump = info.Resolve<short>("incJump") ?? 0;

            OnlyEquip = info.Resolve<bool>("onlyEquip") ?? false;
            TradeBlockEquip = info.Resolve<bool>("equipTradeBlock") ?? false;

            NotExtend = info.Resolve<bool>("notExtend") ?? false;
            SharableOnce = info.Resolve<bool>("sharableOnce") ?? false;

            AppliableKarmaType = info.Resolve<byte>("tradeAvailable") ?? 0;

            SetItemID = info.Resolve<int>("setItemID") ?? 0;
            Durability = info.Resolve<int>("durability") ?? -1;
        }
    }
}