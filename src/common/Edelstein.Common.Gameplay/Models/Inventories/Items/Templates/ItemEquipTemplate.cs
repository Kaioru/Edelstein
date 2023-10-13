using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items.Templates;

public record ItemEquipTemplate : ItemTemplate, IItemEquipTemplate
{
    // public int EnchantCategory { get;  }
    // public int Transform { get;  }
    // public int IUCMax { get;  }

    public ItemEquipTemplate(int id, IDataNode info) : base(id, info)
    {
        ReqSTR = info.ResolveShort("reqSTR") ?? 0;
        ReqDEX = info.ResolveShort("reqDEX") ?? 0;
        ReqINT = info.ResolveShort("reqINT") ?? 0;
        ReqLUK = info.ResolveShort("reqLUK") ?? 0;
        ReqPOP = info.ResolveShort("reqPOP") ?? 0;
        ReqJob = info.ResolveShort("reqJob") ?? 0;
        ReqLevel = info.ResolveByte("reqLevel") ?? 0;

        TUC = info.ResolveByte("tuc") ?? 0;
        IncSTR = info.ResolveShort("incSTR") ?? 0;
        IncDEX = info.ResolveShort("incDEX") ?? 0;
        IncINT = info.ResolveShort("incINT") ?? 0;
        IncLUK = info.ResolveShort("incLUK") ?? 0;
        IncMaxHP = info.ResolveInt("incMHP") ?? 0;
        IncMaxMP = info.ResolveInt("incMMP") ?? 0;
        IncMaxHPr = info.ResolveInt("incMHPr") ?? 0;
        IncMaxMPr = info.ResolveInt("incMMPr") ?? 0;
        IncPAD = info.ResolveShort("incPAD") ?? 0;
        IncMAD = info.ResolveShort("incMAD") ?? 0;
        IncPDD = info.ResolveShort("incPDD") ?? 0;
        IncMDD = info.ResolveShort("incMDD") ?? 0;
        IncACC = info.ResolveShort("incACC") ?? 0;
        IncEVA = info.ResolveShort("incEVA") ?? 0;
        IncCraft = info.ResolveShort("incCraft") ?? 0;
        IncSpeed = info.ResolveShort("incSpeed") ?? 0;
        IncJump = info.ResolveShort("incJump") ?? 0;
        
        AttackSpeed = info.ResolveInt("attackSpeed");

        OnlyEquip = info.ResolveBool("onlyEquip") ?? false;
        TradeBlockEquip = info.ResolveBool("equipTradeBlock") ?? false;

        NotExtend = info.ResolveBool("notExtend") ?? false;
        SharableOnce = info.ResolveBool("sharableOnce") ?? false;

        AppliableKarmaType = info.ResolveByte("tradeAvailable") ?? 0;

        SetItemID = info.ResolveInt("setItemID") ?? 0;
        Durability = info.ResolveInt("durability") ?? -1;
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
    
    public int? AttackSpeed { get; }

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
}
