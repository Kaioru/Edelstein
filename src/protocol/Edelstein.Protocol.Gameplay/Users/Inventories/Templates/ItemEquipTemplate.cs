using Edelstein.Protocol.Parser;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates
{
    public record ItemEquipTemplate : ItemTemplate
    {
        public short ReqSTR { get; init; }
        public short ReqDEX { get; init; }
        public short ReqINT { get; init; }
        public short ReqLUK { get; init; }
        public short ReqPOP { get; init; }
        public short ReqJob { get; init; }
        public byte ReqLevel { get; init; }

        public byte TUC { get; init; }
        public short IncSTR { get; init; }
        public short IncDEX { get; init; }
        public short IncINT { get; init; }
        public short IncLUK { get; init; }
        public int IncMaxHP { get; init; }
        public int IncMaxMP { get; init; }
        public int IncMaxHPr { get; init; }
        public int IncMaxMPr { get; init; }
        public short IncPAD { get; init; }
        public short IncMAD { get; init; }
        public short IncPDD { get; init; }
        public short IncMDD { get; init; }
        public short IncACC { get; init; }
        public short IncEVA { get; init; }
        public short IncCraft { get; init; }
        public short IncSpeed { get; init; }
        public short IncJump { get; init; }

        // fs, swim, tamingmob
        // public int IUC { get; init; }
        // public byte MinGrade { get; init; }

        public bool OnlyEquip { get; init; }
        public bool TradeBlockEquip { get; init; }

        // nirPoison, nirIce, nirFire, nirLight, nirHoly
        // other random stuff

        public bool NotExtend { get; init; }
        public bool SharableOnce { get; init; }

        public byte AppliableKarmaType { get; init; }

        public int SetItemID { get; init; }

        public int Durability { get; init; }
        // public int EnchantCategory { get; init; }
        // public int Transform { get; init; }
        // public int IUCMax { get; init; }

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
