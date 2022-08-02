using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Inventories.Templates;

public record ItemEquipTemplate : ItemTemplate, IItemEquipTemplate
{
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

    public short ReqSTR { get; }
    public short ReqDEX { get; }
    public short ReqINT { get; }
    public short ReqLUK { get; }
    public short ReqPOP { get; }
    public short ReqJob { get; }
    public byte ReqLevel { get; }

    public byte TUC { get; }
    public short IncSTR { get; }
    public short IncDEX { get; }
    public short IncINT { get; }
    public short IncLUK { get; }
    public int IncMaxHP { get; }
    public int IncMaxMP { get; }
    public int IncMaxHPr { get; }
    public int IncMaxMPr { get; }
    public short IncPAD { get; }
    public short IncMAD { get; }
    public short IncPDD { get; }
    public short IncMDD { get; }
    public short IncACC { get; }
    public short IncEVA { get; }
    public short IncCraft { get; }
    public short IncSpeed { get; }
    public short IncJump { get; }

    // fs, swim, tamingmob
    // public int IUC { get;  }
    // public byte MinGrade { get;  }

    public bool OnlyEquip { get; }
    public bool TradeBlockEquip { get; }

    // nirPoison, nirIce, nirFire, nirLight, nirHoly
    // other random stuff

    public bool NotExtend { get; }
    public bool SharableOnce { get; }

    public byte AppliableKarmaType { get; }

    public int SetItemID { get; }

    public int Durability { get; }

    // public int EnchantCategory { get;  }
    // public int Transform { get;  }
    // public int IUCMax { get;  }
}
