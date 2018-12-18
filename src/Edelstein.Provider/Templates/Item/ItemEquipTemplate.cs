using Edelstein.Provider.Parser;

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

        public override void Parse(int id, IDataProperty p)
        {
            base.Parse(id, p);

            ReqSTR = p.Resolve<short>("info/reqSTR") ?? 0;
            ReqDEX = p.Resolve<short>("info/reqDEX") ?? 0;
            ReqINT = p.Resolve<short>("info/reqINT") ?? 0;
            ReqLUK = p.Resolve<short>("info/reqLUK") ?? 0;
            ReqPOP = p.Resolve<short>("info/reqPOP") ?? 0;
            ReqJob = p.Resolve<short>("info/reqJob") ?? 0;
            ReqLevel = p.Resolve<byte>("info/reqLevel") ?? 0;

            TUC = p.Resolve<byte>("info/tuc") ?? 0;
            IncSTR = p.Resolve<short>("info/incSTR") ?? 0;
            IncDEX = p.Resolve<short>("info/incDEX") ?? 0;
            IncINT = p.Resolve<short>("info/incINT") ?? 0;
            IncLUK = p.Resolve<short>("info/incLUK") ?? 0;
            IncMaxHP = p.Resolve<int>("info/incMHP") ?? 0;
            IncMaxMP = p.Resolve<int>("info/incMMP") ?? 0;
            IncMaxHPr = p.Resolve<int>("info/incMHPr") ?? 0;
            IncMaxMPr = p.Resolve<int>("info/incMMPr") ?? 0;
            IncPAD = p.Resolve<short>("info/incPAD") ?? 0;
            IncMAD = p.Resolve<short>("info/incMAD") ?? 0;
            IncPDD = p.Resolve<short>("info/incPDD") ?? 0;
            IncMDD = p.Resolve<short>("info/incMDD") ?? 0;
            IncACC = p.Resolve<short>("info/incACC") ?? 0;
            IncEVA = p.Resolve<short>("info/incEVA") ?? 0;
            IncCraft = p.Resolve<short>("info/incCraft") ?? 0;
            IncSpeed = p.Resolve<short>("info/incSpeed") ?? 0;
            IncJump = p.Resolve<short>("info/incJump") ?? 0;

            OnlyEquip = p.Resolve<bool>("info/onlyEquip") ?? false;
            TradeBlockEquip = p.Resolve<bool>("info/equipTradeBlock") ?? false;

            NotExtend = p.Resolve<bool>("info/notExtend") ?? false;
            SharableOnce = p.Resolve<bool>("info/sharableOnce") ?? false;

            AppliableKarmaType = p.Resolve<byte>("info/tradeAvailable") ?? 0;

            SetItemID = p.Resolve<int>("info/setItemID") ?? 0;
            Durability = p.Resolve<int>("info/durability") ?? -1;
        }
    }
}