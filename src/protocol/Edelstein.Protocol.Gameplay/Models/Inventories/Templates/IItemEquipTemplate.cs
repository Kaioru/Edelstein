namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

public interface IItemEquipTemplate : IItemTemplate
{
    short ReqSTR { get; }
    short ReqDEX { get; }
    short ReqINT { get; }
    short ReqLUK { get; }
    short ReqPOP { get; }
    short ReqJob { get; }
    byte ReqLevel { get; }

    byte TUC { get; }
    short IncSTR { get; }
    short IncDEX { get; }
    short IncINT { get; }
    short IncLUK { get; }
    int IncMaxHP { get; }
    int IncMaxMP { get; }
    int IncMaxHPr { get; }
    int IncMaxMPr { get; }
    short IncPAD { get; }
    short IncMAD { get; }
    short IncPDD { get; }
    short IncMDD { get; }
    short IncACC { get; }
    short IncEVA { get; }
    short IncCraft { get; }
    short IncSpeed { get; }
    short IncJump { get; }

    int? AttackSpeed { get; }
    
    // fs, swim, tamingmob
    // int IUC { get;  }
    // byte MinGrade { get;  }

    bool OnlyEquip { get; }
    bool TradeBlockEquip { get; }

    // nirPoison, nirIce, nirFire, nirLight, nirHoly
    // other random stuff

    bool NotExtend { get; }
    bool SharableOnce { get; }

    byte AppliableKarmaType { get; }

    int SetItemID { get; }

    int Durability { get; }
    // int EnchantCategory { get;  }
    // int Transform { get;  }
    // int IUCMax { get;  }
}
