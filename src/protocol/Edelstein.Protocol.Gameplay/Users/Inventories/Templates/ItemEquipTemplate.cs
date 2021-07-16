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
    }
}
